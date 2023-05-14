using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Bookings.Managers
{
    public interface IWorkspaceConfigurationManager
    {
        Task<WorkspaceConfigurationModel[]> GetWorkspaceConfigurations(string userId, string workspaceId);

        Task<WorkspaceConfigurationModel> GetWorkspaceConfiguration(string configurationId);

        Task<WorkspaceConfigurationModel> CreateWorkspaceConfiguration(SaveWorkspaceConfigurationModel model);

        Task<WorkspaceConfigurationModel> UpdateWorkspaceConfiguration(string configurationId, SaveWorkspaceConfigurationModel model);

        Task DeleteWorkspaceConfiguration(string configurationId);

        Task DeleteWorkspaceConfigurationsForUser(string userId);

        Task DeleteWorkspaceConfigurationsForWorkspace(string workspaceId);
    }

    public class WorkspaceConfigurationManager : IWorkspaceConfigurationManager
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;

        public WorkspaceConfigurationManager(IAmazonDynamoDB dynamoDbClient, IMapper mapper, IIdGenerator idGenerator)
        {
            _dynamoDbClient = dynamoDbClient;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<WorkspaceConfigurationModel[]> GetWorkspaceConfigurations(string userId, string workspaceId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var workspaceConfigurations = await context
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
            using var context = new DynamoDBContext(_dynamoDbClient);

            var configuration = await context.LoadAsync<WorkspaceConfiguration>(configurationId);

            if (configuration == null)
            {
                throw new NotFoundException($"Could not find configuration with id '{configurationId}'!");
            }

            return _mapper.Map<WorkspaceConfigurationModel>(configuration);
        }

        public async Task<WorkspaceConfigurationModel> CreateWorkspaceConfiguration(SaveWorkspaceConfigurationModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var configuration = _mapper.Map<WorkspaceConfiguration>(model);

            configuration.Id = _idGenerator.GenerateId();
            configuration.WorkspaceUser = $"{model.WorkspaceId}-{model.UserId}";

            await context.SaveAsync(configuration);

            return await GetWorkspaceConfiguration(configuration.Id);
        }

        public async Task<WorkspaceConfigurationModel> UpdateWorkspaceConfiguration(string configurationId, SaveWorkspaceConfigurationModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var configuration = await context.LoadAsync<WorkspaceConfiguration>(configurationId);

            if (configuration == null)
            {
                throw new NotFoundException($"Could not find configuration with id '{configurationId}'!");
            }

            _mapper.Map(model, configuration);

            await context.SaveAsync(configuration);

            return await GetWorkspaceConfiguration(configurationId);
        }

        public async Task DeleteWorkspaceConfiguration(string configurationId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            await context.DeleteAsync<WorkspaceConfiguration>(configurationId);
        }

        public async Task DeleteWorkspaceConfigurationsForUser(string userId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            await context.DeleteAsync<WorkspaceConfiguration>(userId, new DynamoDBOperationConfig
            {
                IndexName = "UserId-index"
            });
        }

        public async Task DeleteWorkspaceConfigurationsForWorkspace(string workspaceId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            await context.DeleteAsync<WorkspaceConfiguration>(workspaceId, new DynamoDBOperationConfig
            {
                IndexName = "WorkspaceId-index"
            });
        }
    }
}
