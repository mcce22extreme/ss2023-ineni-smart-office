using AutoMapper;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomModel>();

            CreateMap<SaveRoomModel, Room>();
        }
    }
}
