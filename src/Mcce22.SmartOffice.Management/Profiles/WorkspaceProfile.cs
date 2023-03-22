using AutoMapper;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Profiles
{
    public class WorkspaceProfile : Profile
    {
        public WorkspaceProfile()
        {
            CreateMap<Workspace, WorkspaceModel>();

            CreateMap<SaveWorkspaceModel, Workspace>();
        }
    }
}
