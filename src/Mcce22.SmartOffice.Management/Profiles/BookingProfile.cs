using AutoMapper;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingModel>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.User.Id))
                .ForMember(d => d.WorkspaceId, opt => opt.MapFrom(s => s.Workspace.Id));

            CreateMap<SaveBookingModel, Booking>();
        }
    }
}
