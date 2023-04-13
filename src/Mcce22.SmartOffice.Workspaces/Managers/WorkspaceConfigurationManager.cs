using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Workspaces;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Bookings.Managers
{
    public interface IWorkspaceConfigurationManager
    {
        Task<WorkspaceConfigurationModel[]> GetWorkspaceConfigurations(int userId, int workspaceId);

        Task<WorkspaceConfigurationModel> GetWorkspaceConfiguration(int configurationId);

        Task<WorkspaceConfigurationModel> CreateWorkspaceConfiguration(SaveWorkspaceConfigurationModel model);

        Task<WorkspaceConfigurationModel> UpdateWorkspaceConfiguration(int configurationId, SaveWorkspaceConfigurationModel model);

        Task DeleteWorkspaceConfiguration(int configurationId);
    }

    public class WorkspaceConfigurationManager : IWorkspaceConfigurationManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public WorkspaceConfigurationManager(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<WorkspaceConfigurationModel[]> GetWorkspaceConfigurations(int userId, int workspaceId)
        {
            var configurationsQuery = _dbContext.WorkspaceConfigurations.AsQueryable();

            if (userId > 0)
            {
                configurationsQuery = configurationsQuery.Where(x => x.UserId == userId);
            }

            if (workspaceId > 0)
            {
                configurationsQuery = configurationsQuery.Where(x => x.WorkspaceId == workspaceId);
            }

            var configurations = await configurationsQuery.ToListAsync();

            return configurations.Select(_mapper.Map<WorkspaceConfigurationModel>).ToArray();
        }

        public async Task<WorkspaceConfigurationModel> GetWorkspaceConfiguration(int configurationId)
        {
            var configuration = await _dbContext.WorkspaceConfigurations.FirstOrDefaultAsync(x => x.Id == configurationId);

            if (configuration == null)
            {
                throw new NotFoundException($"Could not find configuration with id '{configurationId}'!");
            }

            return _mapper.Map<WorkspaceConfigurationModel>(configuration);
        }

        public async Task<WorkspaceConfigurationModel> CreateWorkspaceConfiguration(SaveWorkspaceConfigurationModel model)
        {
            var user = _mapper.Map<WorkspaceConfiguration>(model);

            await _dbContext.WorkspaceConfigurations.AddAsync(user);

            await _dbContext.SaveChangesAsync();

            return await GetWorkspaceConfiguration(user.Id);
        }

        public async Task<WorkspaceConfigurationModel> UpdateWorkspaceConfiguration(int configurationId, SaveWorkspaceConfigurationModel model)
        {
            var configuration = await _dbContext.WorkspaceConfigurations.FirstOrDefaultAsync(x => x.Id == configurationId);

            if (configuration == null)
            {
                throw new NotFoundException($"Could not find configuration with id '{configurationId}'!");
            }

            _mapper.Map(model, configuration);

            await _dbContext.SaveChangesAsync();

            return await GetWorkspaceConfiguration(configurationId);
        }

        public async Task DeleteWorkspaceConfiguration(int configurationId)
        {
            var configuration = await _dbContext.WorkspaceConfigurations.FirstOrDefaultAsync(x => x.Id == configurationId);

            if (configuration == null)
            {
                throw new NotFoundException($"Could not find configuration with id '{configurationId}'!");
            }

            _dbContext.WorkspaceConfigurations.Remove(configuration);

            await _dbContext.SaveChangesAsync();
        }
    }
}
