using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Generators;
using Mcce22.SmartOffice.Workspaces.Models;
using Mcce22.SmartOffice.Workspaces.Queries;

namespace Mcce22.SmartOffice.Workspaces.Managers
{
    public class WorkspaceDataManager : IWorkspaceDataManager
    {
        private readonly IDynamoDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;
        private readonly IWeiGenerator _weiGenerator;

        public WorkspaceDataManager(IDynamoDBContext dbContext, IMapper mapper, IIdGenerator idGenerator, IWeiGenerator weiGenerator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _idGenerator = idGenerator;
            _weiGenerator = weiGenerator;
        }

        public async Task<WorkspaceDataModel[]> GetWorkspaceData(WorkspaceDataQuery query)
        {
            var startDate = query.StartDate ?? DateTime.Now.Date;
            var endDate = query.EndDate ?? DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var workspaceData = await _dbContext
                .ScanAsync<WorkspaceData>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            var workspaceDataQuery = workspaceData
                .Where(x => x.Timestamp >= startDate && x.Timestamp <= endDate)
                .OrderByDescending(x => x.Timestamp)
                .AsQueryable();

            return workspaceDataQuery.Select(_mapper.Map<WorkspaceDataModel>).ToArray();
        }

        public async Task<WorkspaceDataModel> CreateWorkspaceData(SaveWorkspaceDataModel model)
        {
            var workspace = await _dbContext.LoadAsync<Workspace>(model.WorkspaceId) ?? throw new NotFoundException($"Could not find workspace with id '{model.WorkspaceId}'!");

            var workspaceData = _mapper.Map(model, new WorkspaceData
            {
                WorkspaceNumber = workspace.WorkspaceNumber,
                RoomNumber = workspace.RoomNumber,
            });

            workspaceData.Id = _idGenerator.GenerateId();
            workspaceData.Wei = _weiGenerator.GenerateWei(workspaceData);

            if (!model.Timestamp.HasValue)
            {
                workspaceData.Timestamp = DateTime.UtcNow;
            }

            await _dbContext.SaveAsync(workspaceData);

            await UpdateWorkspaceWei(workspace, workspaceData);

            return _mapper.Map<WorkspaceDataModel>(workspaceData);
        }

        public async Task DeleteWorkspaceData(string workspaceDataId)
        {
            await _dbContext.DeleteAsync<WorkspaceData>(workspaceDataId);
        }

        private async Task UpdateWorkspaceWei(Workspace workspace, WorkspaceData data)
        {
            var startDate = DateTime.Now.Subtract(TimeSpan.FromDays(10));
            var endDate = DateTime.Now;

            var workspaceData = await _dbContext.QueryAsync<WorkspaceData>(workspace.Id, new DynamoDBOperationConfig
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

            await _dbContext.SaveAsync(workspace);
        }
    }
}
