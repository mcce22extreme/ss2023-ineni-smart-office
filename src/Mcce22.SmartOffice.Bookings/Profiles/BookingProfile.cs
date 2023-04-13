using AutoMapper;
using Mcce22.SmartOffice.Bookings.Entities;
using Mcce22.SmartOffice.Bookings.Models;

namespace Mcce22.SmartOffice.Bookings.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingModel>();

            CreateMap<SaveBookingModel, Booking>();
        }
    }
}
