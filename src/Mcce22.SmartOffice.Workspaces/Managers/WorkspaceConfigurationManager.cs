using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Bookings.Managers
{
    public class WorkspaceConfigurationManager : IWorkspaceConfigurationManager
    {
        private readonly IDynamoDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;

        public WorkspaceConfigurationManager(IDynamoDBContext dbContext, IMapper mapper, IIdGenerator idGenerator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<WorkspaceConfigurationModel[]> GetWorkspaceConfigurations(string userId, string workspaceId)
        {
            var workspaceConfigurations = await _dbContext
                .ScanAsync<WorkspaceConfiguration>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            var filteredResult = workspaceConfigurations.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                filteredResult = filteredResult.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(workspaceId))
            {
                filteredResult = filteredResult.Where(y => y.WorkspaceId == workspaceId);
            }

            return filteredResult.Select(_mapper.Map<WorkspaceConfigurationModel>).ToArray();
        }

        public async Task<WorkspaceConfigurationModel> GetWorkspaceConfiguration(string configurationId)
        {
            var configuration = await _dbContext.LoadAsync<WorkspaceConfiguration>(configurationId);

            return configuration == null
                ? throw new NotFoundException($"Could not find configuration with id '{configurationId}'!")
                : _mapper.Map<WorkspaceConfigurationModel>(configuration);
        }

        public async Task<WorkspaceConfigurationModel> CreateWorkspaceConfiguration(SaveWorkspaceConfigurationModel model)
        {
            var configuration = _mapper.Map<WorkspaceConfiguration>(model);

            configuration.Id = _idGenerator.GenerateId();
            configuration.WorkspaceUser = $"{model.WorkspaceId}-{model.UserId}";

            await _dbContext.SaveAsync(configuration);

            return await GetWorkspaceConfiguration(configuration.Id);
        }

        public async Task<WorkspaceConfigurationModel> UpdateWorkspaceConfiguration(string configurationId, SaveWorkspaceConfigurationModel model)
        {
            var configuration = await _dbContext.LoadAsync<WorkspaceConfiguration>(configurationId) ?? throw new NotFoundException($"Could not find configuration with id '{configurationId}'!");

            _mapper.Map(model, configuration);

            await _dbContext.SaveAsync(configuration);

            return await GetWorkspaceConfiguration(configurationId);
        }

        public async Task DeleteWorkspaceConfiguration(string configurationId)
        {
            await _dbContext.DeleteAsync<WorkspaceConfiguration>(configurationId);
        }

        public async Task DeleteWorkspaceConfigurationsForUser(string userId)
        {
            await _dbContext.DeleteAsync<WorkspaceConfiguration>(userId, new DynamoDBOperationConfig
            {
                IndexName = "UserId-index",
            });
        }

        public async Task DeleteWorkspaceConfigurationsForWorkspace(string workspaceId)
        {
            await _dbContext.DeleteAsync<WorkspaceConfiguration>(workspaceId, new DynamoDBOperationConfig
            {
                IndexName = "WorkspaceId-index",
            });
        }
    }
}
