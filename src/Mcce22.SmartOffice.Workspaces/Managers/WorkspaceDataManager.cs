using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Workspaces.Entities;
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
            var workspaceData = await _dbContext
                .ScanAsync<WorkspaceData>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            var workspaceDataQuery = workspaceData
                .OrderByDescending(x => x.Timestamp)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.WorkspaceId))
            {
                workspaceDataQuery = workspaceDataQuery.Where(x => x.WorkspaceId == query.WorkspaceId);
            }

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
            workspaceData.Wei = _weiGenerator.GenerateWei(workspaceData.Temperature, workspaceData.Humidity, workspaceData.Co2Level);
            workspace.Wei = workspaceData.Wei;

            if (!model.Timestamp.HasValue)
            {
                workspaceData.Timestamp = DateTime.UtcNow;
            }

            await _dbContext.SaveAsync(workspaceData);
            await _dbContext.SaveAsync(workspace);

            return _mapper.Map<WorkspaceDataModel>(workspaceData);
        }

        public async Task DeleteWorkspaceData(string workspaceDataId)
        {
            await _dbContext.DeleteAsync<WorkspaceData>(workspaceDataId);
        }

        public async Task DeleteAll()
        {
            var workspaceData = await _dbContext
                .ScanAsync<WorkspaceData>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            foreach (var data in workspaceData)
            {
                await _dbContext.DeleteAsync(data);
            }
        }
    }
}
