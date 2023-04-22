using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;
using Mcce22.SmartOffice.Workspaces.Queries;

namespace Mcce22.SmartOffice.Workspaces.Managers
{
    public interface IWorkspaceDataManager
    {
        Task<WorkspaceDataModel[]> GetWorkspaceData(WorkspaceDataQuery query);

        Task<WorkspaceDataModel> CreateWorkspaceData(SaveWorkspaceDataModel model);

        Task DeleteWorkspaceData(string workspaceDataId);
    }

    public class WorkspaceDataManager : IWorkspaceDataManager
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;

        public WorkspaceDataManager(IAmazonDynamoDB dynamoDbClient, IMapper mapper, IIdGenerator idGenerator)
        {
            _dynamoDbClient = dynamoDbClient;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<WorkspaceDataModel[]> GetWorkspaceData(WorkspaceDataQuery query)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var startDate = query.StartDate ?? DateTime.Now.Date;
            var endDate = query.EndDate ?? DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var workspaceData = await context
                .ScanAsync<WorkspaceData>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            var workspaceDataQuery =workspaceData
                .OrderBy(x => x.Timestamp)
                .AsQueryable();

            if (query.StartDate.HasValue)
            {
                workspaceDataQuery = workspaceDataQuery.Where(x => x.Timestamp >= query.StartDate.Value);
            }

            if (query.EndDate.HasValue)
            {
                workspaceDataQuery = workspaceDataQuery.Where(x => x.Timestamp <= query.StartDate.Value);
            }

            return workspaceDataQuery.Select(_mapper.Map<WorkspaceDataModel>).ToArray();
        }

        public async Task<WorkspaceDataModel> CreateWorkspaceData(SaveWorkspaceDataModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var workspace = await context.LoadAsync<Workspace>(model.WorkspaceId);

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{model.WorkspaceId}'!");
            }

            var workspaceData = _mapper.Map(model, new WorkspaceData
            {
                WorkspaceNumber = workspace.WorkspaceNumber,
                RoomNumber = workspace.RoomNumber
            });

            workspaceData.Id = _idGenerator.GenerateId();

            if (!model.Timestamp.HasValue)
            {
                workspaceData.Timestamp = DateTime.UtcNow;
            }

            await context.SaveAsync(workspaceData);

            return _mapper.Map<WorkspaceDataModel>(workspaceData);
        }

        public async Task DeleteWorkspaceData(string workspaceDataId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            await context.DeleteAsync<WorkspaceData>(workspaceDataId);
        }
    }
}
