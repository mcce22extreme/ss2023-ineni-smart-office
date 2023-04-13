using AutoMapper;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Workspaces.Profiles
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
