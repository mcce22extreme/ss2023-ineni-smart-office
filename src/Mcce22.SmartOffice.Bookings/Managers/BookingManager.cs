using AutoMapper;
using FluentValidation;
using Mcce22.SmartOffice.Bookings.Entities;
using Mcce22.SmartOffice.Bookings.Models;
using Mcce22.SmartOffice.Bookings.Services;
using Mcce22.SmartOffice.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Bookings.Managers
{
    public interface IBookingManager
    {
        Task<BookingModel[]> GetBookings();

        Task<BookingModel> GetBooking(int bookingId);

        Task<BookingModel> CreateBooking(SaveBookingModel model);

        Task DeleteBooking(int bookingId);

        Task<BookingModel> ActivateBooking(int bookingId);

        Task ProcessBookings();
    }

    public class BookingManager : IBookingManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public BookingManager(AppDbContext dbContext, IMapper mapper, IEmailService emailService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<BookingModel[]> GetBookings()
        {
            var bookings = await _dbContext.Bookings.ToListAsync();

            return bookings.Select(_mapper.Map<BookingModel>).ToArray();
        }

        public async Task<BookingModel> GetBooking(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .OrderBy(x => x.StartDateTime)
                .FirstOrDefaultAsync(x => x.Id == bookingId);

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

            var collision = await CheckAvailability(model.WorkspaceId, model.StartDateTime, model.EndDateTime);

            if (collision)
            {
                throw new ValidationException("A collision occurred during booking the workspace! The workspace has already been booked by another user in the specified time.");
            }

            var booking = _mapper.Map<Booking>(model);

            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();

            return await GetBooking(booking.Id);
        }

        private async Task<bool> CheckAvailability(int workspaceId, DateTime startDate, DateTime endDate)
        {
            var collision = await _dbContext.Bookings
                .Where(x => x.WorkspaceId == workspaceId &&
                    ((startDate == x.StartDateTime && endDate == x.EndDateTime) ||
                    (startDate > x.StartDateTime && startDate < x.EndDateTime) ||
                    (endDate > x.StartDateTime && endDate < x.EndDateTime) ||
                    (startDate < x.StartDateTime && endDate > x.EndDateTime)))
                .AnyAsync();

            return collision;
        }

        public async Task DeleteBooking(int bookingId)
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId);

            if (booking == null)
            {
                throw new NotFoundException($"Could not find booking with id '{bookingId}'!");
            }

            _dbContext.Bookings.Remove(booking);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<BookingModel> ActivateBooking(int bookingId)
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId);

            if (booking == null)
            {
                throw new NotFoundException($"Could not find booking with id '{bookingId}'!");
            }

            //// Check if booking can be activated
            //if (booking.StartDateTime > DateTime.Now)
            //{
            //    throw new ValidationException("Can't activate booking since startdate is still in future!");
            //}

            //if (booking.EndDateTime < DateTime.Now)
            //{
            //    throw new ValidationException("Can't activate booking since enddate is in the past!");
            //}

            if (booking.Activated)
            {
                throw new ValidationException("Booking already has been activated!");
            }

            booking.Activated = true;

            await _dbContext.SaveChangesAsync();

            return await GetBooking(bookingId);
        }

        public async Task ProcessBookings()
        {
            // Get bookings for today
            var bookings = await _dbContext.Bookings
                .Where(x => x.StartDateTime.Date == DateTime.Now.Date)
                .ToListAsync();            

            foreach (var booking in bookings)
            {
                await _emailService.SendMail(booking);

                booking.InvitationSent = true;                
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
