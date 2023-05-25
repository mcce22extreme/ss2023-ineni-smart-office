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
        private readonly IDynamoDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;

        public BookingManager(IDynamoDBContext dbContext, IMapper mapper, IIdGenerator idGenerator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<BookingModel[]> GetBookings()
        {
            var bookings = await _dbContext
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
            var booking = await _dbContext.LoadAsync<Booking>(bookingId);

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

            var user = await _dbContext.LoadAsync<User>(model.UserId);
            if (user == null)
            {
                throw new NotFoundException($"Could not find user with id '{model.UserId}'!");
            }

            var workspace = await _dbContext.LoadAsync<Workspace>(model.WorkspaceId);
            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{model.WorkspaceId}'!");
            }

            var booking = _mapper.Map<Booking>(model);

            booking.Id = _idGenerator.GenerateId();
            booking.FirstName = user.FirstName;
            booking.LastName = user.LastName;
            booking.UserName = user.UserName;
            booking.Email = user.Email;
            booking.WorkspaceNumber = workspace.WorkspaceNumber;
            booking.RoomNumber = workspace.RoomNumber;

            await _dbContext.SaveAsync(booking);

            return await GetBooking(booking.Id);
        }

        private async Task<bool> CheckAvailability(string workspaceId, DateTime startDateTime, DateTime endDateTime)
        {
            var bookings = await _dbContext.QueryAsync<Booking>(
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
            await _dbContext.DeleteAsync<Booking>(bookingId);
        }       
    }
}
