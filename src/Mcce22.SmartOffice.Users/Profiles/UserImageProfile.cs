using AutoMapper;
using Mcce22.SmartOffice.Users.Entities;
using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Profiles
{
    public class UserImageProfile : Profile
    {
        public UserImageProfile()
        {
            CreateMap<UserImage, UserImageModel>();

            CreateMap<SaveUserImageModel, UserImage>();
        }
    }
}
