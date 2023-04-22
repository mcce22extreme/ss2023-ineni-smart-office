﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using FluentValidation;
using Mcce22.SmartOffice.Bookings.Entities;
using Mcce22.SmartOffice.Bookings.Models;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;

namespace Mcce22.SmartOffice.Bookings.Managers
{
    public interface IBookingManager
    {
        Task<BookingModel[]> GetBookings();

        Task<BookingModel> GetBooking(string bookingId);

        Task<BookingModel> CreateBooking(SaveBookingModel model);

        Task DeleteBooking(string bookingId);
    }

    public class BookingManager : IBookingManager
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;

        public BookingManager(IAmazonDynamoDB dynamoDbClient, IMapper mapper, IIdGenerator idGenerator)
        {
            _dynamoDbClient = dynamoDbClient;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<BookingModel[]> GetBookings()
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var bookings = await context
                .ScanAsync<Booking>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            return bookings
                .OrderBy(x => x.StartDate)
                .ThenBy(x => x.StartTime)
                .Select(_mapper.Map<BookingModel>)
                .ToArray();
        }

        public async Task<BookingModel> GetBooking(string bookingId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var booking = await context.LoadAsync<Booking>(bookingId);

            if (booking == null)
            {
                throw new NotFoundException($"Could not find booking with id '{bookingId}'!");
            }

            return _mapper.Map<BookingModel>(booking);
        }

        public async Task<BookingModel> CreateBooking(SaveBookingModel model)
        {
            // Validate booking
            if (model.StartDateTime > model.EndDateTime)
            {
                throw new ValidationException("End date of booking must not be before start date of booking!");
            }

            if (model.StartDateTime.Date != model.EndDateTime.Date)
            {
                throw new ValidationException("Stard date and end date must not span multiple days!");
            }

            var collision = await CheckAvailability(model.WorkspaceId, model.StartDateTime, model.EndDateTime);

            if (collision)
            {
                throw new ValidationException("A collision occurred during booking the workspace! The workspace has already been booked by another user in the specified time.");
            }

            var booking = _mapper.Map<Booking>(model);

            booking.Id = _idGenerator.GenerateId();

            using var context = new DynamoDBContext(_dynamoDbClient);

            await context.SaveAsync(booking);

            return await GetBooking(booking.Id);
        }

        private async Task<bool> CheckAvailability(string workspaceId, DateTime startDateTime, DateTime endDateTime)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var bookings = await context.QueryAsync<Booking>(
                DateOnly.FromDateTime(startDateTime),
                new DynamoDBOperationConfig
                {
                    IndexName = $"{nameof(Booking.StartDate)}-index"
                })
                .GetRemainingAsync();

            var startTime = TimeOnly.FromDateTime(startDateTime);
            var endTime = TimeOnly.FromDateTime(endDateTime);

            var collision = bookings
                .Where(x => x.WorkspaceId == workspaceId &&
                    ((startTime == x.StartTime && endTime == x.EndTime) ||
                    (startTime > x.StartTime && startTime < x.EndTime) ||
                    (endTime > x.StartTime && endTime < x.EndTime) ||
                    (startTime < x.StartTime && endTime > x.EndTime)))
                .Any();

            return collision;
        }

        public async Task DeleteBooking(string bookingId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            await context.DeleteAsync<Booking>(bookingId);
        }

        //public async Task<BookingModel> ActivateBooking(int bookingId)
        //{
        //    var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId);

        //    if (booking == null)
        //    {
        //        throw new NotFoundException($"Could not find booking with id '{bookingId}'!");
        //    }

        //    //// Check if booking can be activated
        //    //if (booking.StartDateTime > DateTime.Now)
        //    //{
        //    //    throw new ValidationException("Can't activate booking since startdate is still in future!");
        //    //}

        //    //if (booking.EndDateTime < DateTime.Now)
        //    //{
        //    //    throw new ValidationException("Can't activate booking since enddate is in the past!");
        //    //}

        //    if (booking.Activated)
        //    {
        //        throw new ValidationException("Booking already has been activated!");
        //    }

        //    booking.Activated = true;

        //    await _dbContext.SaveChangesAsync();

        //    return await GetBooking(bookingId);
        //}

        //public async Task ProcessBookings()
        //{
        //    // Get bookings for today
        //    var bookings = await _dbContext.Bookings
        //        .Where(x => x.StartDateTime.Date == DateTime.Now.Date)
        //        .ToListAsync();

        //    foreach (var booking in bookings)
        //    {
        //        await _emailService.SendMail(booking);

        //        booking.InvitationSent = true;
        //    }

        //    await _dbContext.SaveChangesAsync();
        //}
    }
}
