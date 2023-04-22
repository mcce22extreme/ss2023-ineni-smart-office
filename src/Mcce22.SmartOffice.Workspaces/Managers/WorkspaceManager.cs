using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Workspaces.Managers
{
    public interface IWorkspaceManager
    {
        Task<WorkspaceModel[]> GetWorkspaces();

        Task<WorkspaceModel> GetWorkspace(string workspaceId);

        Task<WorkspaceModel> CreateWorkspace(SaveWorkspaceModel model);

        Task<WorkspaceModel> UpdateWorkspace(string workspaceId, SaveWorkspaceModel model);

        Task DeleteWorkspace(string workspaceId);
    }

    public class WorkspaceManager : IWorkspaceManager
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;

        public WorkspaceManager(IAmazonDynamoDB dynamoDbClient, IMapper mapper, IIdGenerator idGenerator)
        {
            _dynamoDbClient = dynamoDbClient;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<WorkspaceModel[]> GetWorkspaces()
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var workspaces = await context
                .ScanAsync<Workspace>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            return workspaces
                .OrderBy(x => x.WorkspaceNumber)
                .Select(_mapper.Map<WorkspaceModel>)
                .ToArray();
        }

        public async Task<WorkspaceModel> GetWorkspace(string workspaceId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var workspace = await context.LoadAsync<Workspace>(workspaceId);

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{workspaceId}'!");
            }

            return _mapper.Map<WorkspaceModel>(workspace);
        }

        public async Task<WorkspaceModel> CreateWorkspace(SaveWorkspaceModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var workspace = _mapper.Map<Workspace>(model);

            workspace.Id = _idGenerator.GenerateId();

            await context.SaveAsync(workspace);

            return await GetWorkspace(workspace.Id);
        }

        public async Task<WorkspaceModel> UpdateWorkspace(string workspaceId, SaveWorkspaceModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var workspace = await context.LoadAsync<Workspace>(workspaceId);

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{workspaceId}'!");
            }

            _mapper.Map(model, workspace);

            await context.SaveAsync(workspace);

            return await GetWorkspace(workspaceId);
        }

        public async Task DeleteWorkspace(string workspaceId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            await context.DeleteAsync<Workspace>(workspaceId);
        }
    }
}
