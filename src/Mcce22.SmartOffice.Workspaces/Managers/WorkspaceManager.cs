using AutoMapper;
using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Workspaces.Managers
{
    public interface IWorkspaceManager
    {
        Task<WorkspaceModel[]> GetWorkspaces();

        Task<WorkspaceModel> GetWorkspace(int workspaceId);

        Task<WorkspaceModel> CreateWorkspace(SaveWorkspaceModel model);

        Task<WorkspaceModel> UpdateWorkspace(int workspaceId, SaveWorkspaceModel model);

        Task DeleteWorkspace(int workspaceId);
    }

    public class WorkspaceManager : IWorkspaceManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IWorkspaceConfigurationManager _workspaceConfigurationManager;

        public WorkspaceManager(AppDbContext dbContext, IMapper mapper, IWorkspaceConfigurationManager workspaceConfigurationManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _workspaceConfigurationManager = workspaceConfigurationManager;
        }

        public async Task<WorkspaceModel[]> GetWorkspaces()
        {
            var workspaces = await _dbContext.Workspaces
                .OrderBy(x => x.WorkspaceNumber)
                .ToListAsync();

            return workspaces.Select(_mapper.Map<WorkspaceModel>).ToArray();
        }

        public async Task<WorkspaceModel> GetWorkspace(int workspaceId)
        {
            var workspace = await _dbContext.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId);

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{workspaceId}'!");
            }

            return _mapper.Map<WorkspaceModel>(workspace);
        }

        public async Task<WorkspaceModel> CreateWorkspace(SaveWorkspaceModel model)
        {
            var workspace = _mapper.Map<Workspace>(model);

            await _dbContext.Workspaces.AddAsync(workspace);

            await _dbContext.SaveChangesAsync();

            return await GetWorkspace(workspace.Id);
        }

        public async Task<WorkspaceModel> UpdateWorkspace(int workspaceId, SaveWorkspaceModel model)
        {
            var workspace = await _dbContext.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId);

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{workspaceId}'!");
            }

            _mapper.Map(model, workspace);

            await _dbContext.SaveChangesAsync();

            return await GetWorkspace(workspaceId);
        }

        public async Task DeleteWorkspace(int workspaceId)
        {
            var workspace = await _dbContext.Workspaces.FirstOrDefaultAsync(x => x.Id == workspaceId);

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{workspaceId}'!");
            }

            var configurations = _dbContext.WorkspaceConfigurations.Where(x => x.WorkspaceId == workspaceId).ToList();

            _dbContext.Workspaces.Remove(workspace);

            foreach (var configuration in configurations)
            {
                _dbContext.WorkspaceConfigurations.Remove(configuration);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
