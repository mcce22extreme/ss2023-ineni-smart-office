using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.DataIngress.Entities;
using Mcce22.SmartOffice.DataIngress.Generators;
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
                NoiseLevel = model.NoiseLevel,
                Co2Level = model.Co2Level,
            };

            workspaceData.Wei = _weiGenerator.GenerateWei(workspaceData);

            await context.SaveAsync(workspaceData);

            await UpdateWorkspaceWei(context, workspace, workspaceData);
        }

        private async Task UpdateWorkspaceWei(DynamoDBContext context, Workspace workspace, WorkspaceData data)
        {
            var startDate = DateTime.Now.Subtract(TimeSpan.FromDays(10));
            var endDate = DateTime.Now;

            var workspaceData = await context.QueryAsync<WorkspaceData>(workspace.Id, new DynamoDBOperationConfig
            {
                IndexName = $"{nameof(WorkspaceData.WorkspaceId)}-index",
            }).GetRemainingAsync();

            var workspaceDataRange = workspaceData
                .Where(x => x.Timestamp >= startDate && x.Timestamp <= endDate)
                .OrderBy(x => x.Timestamp)
                .AsQueryable()
                .ToArray();

            if (workspaceDataRange.Length > 0)
            {
                var avgWei = (int)Math.Round(workspaceDataRange.Select(x => x.Wei).Average());
                workspace.Wei = avgWei > 100 ? 100 : avgWei;
            }
            else
            {
                workspace.Wei = data.Wei;
            }

            await context.SaveAsync(workspace);
        }
    }
}
