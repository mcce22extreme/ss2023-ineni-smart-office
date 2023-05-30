using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.DataIngress.Entities;
using Mcce22.SmartOffice.DataIngress.Models;

namespace Mcce22.SmartOffice.DataIngress.Managers
{
    public class WorkspaceDataManager : IWorkspaceDataManager
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly IIdGenerator _idGenerator;

        public WorkspaceDataManager(IAmazonDynamoDB dynamoDbClient, IIdGenerator idGenerator)
        {
            _dynamoDbClient = dynamoDbClient;
            _idGenerator = idGenerator;
        }

        public async Task CreateWorkspaceData(SaveWorkspaceDataModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var workspace = default(Workspace);

            if (model.WorkspaceId == null)
            {
                var workspaces = await context
                    .ScanAsync<Workspace>(Array.Empty<ScanCondition>())
                    .GetRemainingAsync();

                workspace = workspaces.FirstOrDefault();
            }
            else
            {
                workspace = await context.LoadAsync<Workspace>(model.WorkspaceId);
            }

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{model.WorkspaceId}'!");
            }

            var workspaceData = new WorkspaceData
            {
                Id = _idGenerator.GenerateId(),
                Timestamp = DateTime.UtcNow,
                WorkspaceId = workspace.Id,
                WorkspaceNumber = workspace.WorkspaceNumber,
                RoomNumber = workspace.RoomNumber,
                Temperature = model.Temperature,
            };

            await context.SaveAsync(workspaceData);
        }
    }
}
