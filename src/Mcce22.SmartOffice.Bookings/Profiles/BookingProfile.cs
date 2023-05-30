using AutoMapper;
using Mcce22.SmartOffice.Bookings.Entities;
using Mcce22.SmartOffice.Bookings.Models;

namespace Mcce22.SmartOffice.Bookings.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingModel>()
                .ForMember(d => d.StartDateTime, opt => opt.MapFrom(s => s.StartDate.ToDateTime(s.StartTime)))
                .ForMember(d => d.EndDateTime, opt => opt.MapFrom(s => s.EndDate.ToDateTime(s.EndTime)));

            CreateMap<SaveBookingModel, Booking>()
                .ForMember(d => d.StartDate, opt => opt.MapFrom(s => DateOnly.FromDateTime(s.StartDateTime)))
                .ForMember(d => d.StartTime, opt => opt.MapFrom(s => TimeOnly.FromTimeSpan(s.StartDateTime.TimeOfDay)))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(s => DateOnly.FromDateTime(s.EndDateTime)))
                .ForMember(d => d.EndTime, opt => opt.MapFrom(s => TimeOnly.FromTimeSpan(s.EndDateTime.TimeOfDay)));
        }
    }
}
