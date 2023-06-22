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
        private readonly IWeiGenerator _weiGenerator;

        public WorkspaceDataManager(IAmazonDynamoDB dynamoDbClient, IIdGenerator idGenerator, IWeiGenerator weiGenerator)
        {
            _dynamoDbClient = dynamoDbClient;
            _idGenerator = idGenerator;
            _weiGenerator = weiGenerator;
        }

        public async Task CreateWorkspaceData(SaveWorkspaceDataModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var workspace = default(Workspace);

            if (string.IsNullOrEmpty(model.WorkspaceNumber))
            {
                var workspaces = await context
                    .ScanAsync<Workspace>(Array.Empty<ScanCondition>())
                    .GetRemainingAsync();

                workspace = workspaces.FirstOrDefault();
            }
            else
            {
                var result = await context.QueryAsync<Workspace>(model.WorkspaceNumber, new DynamoDBOperationConfig
                {
                    IndexName = $"{nameof(Workspace.WorkspaceNumber)}-index",
                }).GetRemainingAsync();

                workspace = result.FirstOrDefault();
            }

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace '{model.WorkspaceNumber}'!");
            }

            var workspaceData = new WorkspaceData
            {
                Id = _idGenerator.GenerateId(),
                Timestamp = DateTime.UtcNow,
                WorkspaceId = workspace.Id,
                WorkspaceNumber = workspace.WorkspaceNumber,
                RoomNumber = workspace.RoomNumber,
                Temperature = model.Temperature,
                Humidity = model.Humidity,
                Co2Level = model.Co2Level,
            };

            workspaceData.Wei = _weiGenerator.GenerateWei(workspaceData.Temperature, workspaceData.Humidity, workspaceData.Co2Level);
            workspace.Wei = workspaceData.Wei;

            await context.SaveAsync(workspaceData);
            await context.SaveAsync(workspace);
        }
    }
}
