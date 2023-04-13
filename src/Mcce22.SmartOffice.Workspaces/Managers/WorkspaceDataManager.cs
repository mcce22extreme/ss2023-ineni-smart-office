using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Workspaces.Entities;
using Mcce22.SmartOffice.Workspaces.Models;
using Mcce22.SmartOffice.Workspaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Workspaces.Managers
{
    public interface IWorkspaceDataManager
    {
        Task<WorkspaceDataModel[]> GetWorkspaceData(WorkspaceDataQuery query);

        Task<WorkspaceDataModel> CreateWorkspaceData(SaveWorkspaceDataModel model);

        Task DeleteWorkspaceData(int workspaceDataId);
    }

    public class WorkspaceDataManager : IWorkspaceDataManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public WorkspaceDataManager(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<WorkspaceDataModel[]> GetWorkspaceData(WorkspaceDataQuery query)
        {
            var dataQuery = _dbContext.WorkspaceData
                .OrderBy(x => x.Timestamp)
                .AsQueryable();

            if (query.StartDate.HasValue)
            {
                dataQuery = dataQuery.Where(x => x.Timestamp >= query.StartDate.Value);
            }

            if (query.EndDate.HasValue)
            {
                dataQuery = dataQuery.Where(x => x.Timestamp <= query.StartDate.Value);
            }

            var data = await dataQuery.ToListAsync();

            return data.Select(_mapper.Map<WorkspaceDataModel>).ToArray();
        }

        public async Task<WorkspaceDataModel> CreateWorkspaceData(SaveWorkspaceDataModel model)
        {
            var data = _mapper.Map<WorkspaceData>(model);

            if (!model.Timestamp.HasValue)
            {
                data.Timestamp = DateTime.UtcNow;
            }

            var workspace = await _dbContext.Workspaces.FirstOrDefaultAsync(x => x.Id == model.WorkspaceId);

            if (workspace == null)
            {
                throw new NotFoundException($"Could not find workspace with id '{model.WorkspaceId}'!");
            }

            data.WorkspaceNumber = workspace.WorkspaceNumber;
            data.RoomNumber = workspace.RoomNumber;

            await _dbContext.WorkspaceData.AddAsync(data);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<WorkspaceDataModel>(data);
        }

        public async Task DeleteWorkspaceData(int workspaceDataId)
        {
            var data = await _dbContext.WorkspaceData.FirstOrDefaultAsync(x => x.Id == workspaceDataId);

            if (data == null)
            {
                throw new NotFoundException($"Could not find workspace data with id '{workspaceDataId}'!");
            }

            _dbContext.WorkspaceData.Remove(data);

            await _dbContext.SaveChangesAsync();
        }
    }
}
