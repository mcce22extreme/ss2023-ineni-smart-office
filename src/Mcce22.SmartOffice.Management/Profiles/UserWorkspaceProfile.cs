using AutoMapper;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Profiles
{
    public class UserWorkspaceProfile : Profile
    {
        public UserWorkspaceProfile()
        {
            CreateMap<UserWorkspace, UserWorkspaceModel>();

            CreateMap<SaveUserWorkspaceModel, UserWorkspace>();
        }
    }
}
