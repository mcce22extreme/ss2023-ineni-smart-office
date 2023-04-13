using AutoMapper;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Workspaces.Profiles
{
    public class WorkspaceDataProfile : Profile
    {
        public WorkspaceDataProfile()
        {
            CreateMap<WorkspaceData, WorkspaceDataModel>();

            CreateMap<SaveWorkspaceDataModel, WorkspaceData>();
        }
    }
}
