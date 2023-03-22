using AutoMapper;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>();

            CreateMap<SaveUserModel, User>();
        }
    }
}
