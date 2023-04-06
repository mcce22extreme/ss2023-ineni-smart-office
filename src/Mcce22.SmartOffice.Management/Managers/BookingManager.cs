using AutoMapper;
using FluentValidation;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Exceptions;
using Mcce22.SmartOffice.Management.Models;
using Mcce22.SmartOffice.Management.Queries;
using Mcce22.SmartOffice.Management.Services;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management.Managers
{
    public interface IBookingManager
    {
        Task<BookingModel[]> GetBookings(BookingQuery query);

        Task<BookingModel> GetBooking(int bookingId);

        Task<BookingModel> CreateBooking(SaveBookingModel model);

        Task DeleteBooking(int bookingId);

        Task<CheckAvailabilityModel> CheckAvailability(CheckAvailabilityQuery query);

        Task ActivateBooking(int bookingId);

        Task ProcessBookings();
    }

    public class BookingManager : IBookingManager
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public BookingManager(AppDbContext appDbContext, IMapper mapper, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<BookingModel[]> GetBookings(BookingQuery query)
        {
            var bookingsQuery = _appDbContext.Bookings
                .Select(x => new BookingModel
                {
                    Id = x.Id,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,
                    WorkspaceId = x.Workspace.Id,
                    WorkspaceNumber = x.Workspace.WorkspaceNumber,
                    RoomNumber = x.Workspace.RoomNumber,
                    UserId = x.User.Id,
                    InvitationSent = x.InvitationSent,
                    Activated = x.Activated,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName
                })
                .OrderBy(x => x.StartDateTime)
                .AsQueryable();

            if (query.WorkspaceId > 0)
            {
                bookingsQuery = bookingsQuery.Where(x => x.WorkspaceId == query.WorkspaceId);
            }

            if (query.UserId > 0)
            {
                bookingsQuery.Where(x => x.UserId == query.UserId);
            }

            var bookings = await bookingsQuery.ToListAsync();

            return bookings.ToArray();
        }

        public async Task<BookingModel> GetBooking(int bookingId)
        {
            var booking = await _appDbContext.Bookings
                .Select(x => new BookingModel
                {
                    Id = x.Id,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,
                    WorkspaceId = x.Workspace.Id,
                    WorkspaceNumber = x.Workspace.WorkspaceNumber,
                    RoomNumber = x.Workspace.RoomNumber,
                    UserId = x.User.Id,
                    InvitationSent = x.InvitationSent,
                    Activated = x.Activated,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName
                })
                .FirstOrDefaultAsync(x => x.Id == bookingId);

            if (booking == null)
            {
                throw new NotFoundException($"Could not find booking with id '{booking}'!");
            }

            return _mapper.Map<BookingModel>(booking);
        }

        private async Task<bool> CheckAvailability(int workspaceId, DateTime startDateTime, DateTime endDateTime)
        {
            var collision = await _appDbContext.Bookings
                .Where(x => x.Workspace.Id == workspaceId &&
                    ((startDateTime == x.StartDateTime && endDateTime == x.EndDateTime) ||
                    (startDateTime > x.StartDateTime && startDateTime < x.EndDateTime) ||
                    (endDateTime > x.StartDateTime && endDateTime < x.EndDateTime) ||
                    (startDateTime < x.StartDateTime && endDateTime > x.EndDateTime)))
                .AnyAsync();

            return collision;
        }

        public async Task<BookingModel> CreateBooking(SaveBookingModel model)
        {
            // Validate booking
            if (model.StartDateTime < DateTime.Now || model.EndDateTime < DateTime.Now)
            {
                throw new ValidationException("Start or end date of booking must not be in the past!");
            }

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

            booking.User = await _appDbContext.Users.FindAsync(model.UserId);
            booking.Workspace = await _appDbContext.Workspaces.FindAsync(model.WorkspaceId);

            await _appDbContext.Bookings.AddAsync(booking);
            await _appDbContext.SaveChangesAsync();

            return await GetBooking(booking.Id);
        }

        public async Task DeleteBooking(int bookingId)
        {
            var booking = await _appDbContext.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                throw new NotFoundException($"Could not find booking with id '{booking}'!");
            }

            _appDbContext.Bookings.Remove(booking);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<CheckAvailabilityModel> CheckAvailability(CheckAvailabilityQuery query)
        {
            var collision = await CheckAvailability(query.WorkspaceId, query.StartDateTime, query.EndDateTime);

            return new CheckAvailabilityModel
            {
                WorkspaceId = query.WorkspaceId,
                StartDateTime = query.StartDateTime,
                EndDateTime = query.EndDateTime,
                Available = !collision
            };
        }

        public async Task ActivateBooking(int bookingId)
        {
            var booking = await _appDbContext.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                throw new NotFoundException($"Could not find booking with id '{booking}'!");
            }

            // Check if booking can be activated
            if (booking.StartDateTime > DateTime.Now)
            {
                throw new ValidationException("Can't activate booking since startdate is still in future!");
            }

            if (booking.EndDateTime < DateTime.Now)
            {
                throw new ValidationException("Can't activate booking since enddate is in the past!");
            }

            if (booking.Activated)
            {
                throw new ValidationException("Booking already has been activated!");
            }
        }

        public async Task ProcessBookings()
        {
            var dateTimeNow = DateTime.Now;
            var startDate = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 0,0,0);
            var endDate = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 23,59,59);

            // Get bookings for today
            var bookings = await _appDbContext.Bookings
                .Include(x => x.User)
                .Include(x => x.Workspace)
                .Where(x => !x.Activated && !x.InvitationSent &&
                    (x.StartDateTime >= startDate && x.StartDateTime <= endDate))
                .ToListAsync();

            foreach (var booking in bookings)
            {
                await _emailService.SendMail(booking.User, booking.Workspace);

                booking.InvitationSent = true;
            }

            await _appDbContext.SaveChangesAsync();
        }
    }
}
