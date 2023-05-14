using AutoMapper;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Workspaces.Profiles
{
    public class WorkspaceConfigurationProfile : Profile
    {
        public WorkspaceConfigurationProfile()
        {
            CreateMap<WorkspaceConfiguration, WorkspaceConfigurationModel>();

            CreateMap<SaveWorkspaceConfigurationModel, WorkspaceConfiguration>();
        }
    }
}
