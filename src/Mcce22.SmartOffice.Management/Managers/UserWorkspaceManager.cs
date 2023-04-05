using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management.Managers
{
    public interface IUserWorkspaceManager
    {
        Task<UserWorkspaceModel[]> GetUserWorkspaces(int userId, int workspaceId);

        Task<UserWorkspaceModel> GetUserWorkspace(int userWorkspaceId);

        Task<UserWorkspaceModel> CreateUserWorkspace(SaveUserWorkspaceModel model);

        Task<UserWorkspaceModel> UpdateUserWorkspace(SaveUserWorkspaceModel model);

        Task DeleteUserWorkspace(int userId, int workspaceId);
    }

    public class UserWorkspaceManager : IUserWorkspaceManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserWorkspaceManager(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserWorkspaceModel[]> GetUserWorkspaces(int userId, int workspaceId)
        {
            var userWorkspaceQuery = _dbContext.UserWorkspaces
                .Select(x => new UserWorkspaceModel
                {
                    Id = x.Id,
                    DeskHeight = x.DeskHeight,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,
                    UserId = x.User.Id,
                    WorkspaceId = x.Workspace.Id,
                    WorkspaceNumber = x.Workspace.WorkspaceNumber,
                    RoomNumber = x.Workspace.RoomNumber
                });

            if(userId > 0)
            {
                userWorkspaceQuery = userWorkspaceQuery.Where(x => x.UserId == userId);
            }

            if(workspaceId > 0)
            {
                userWorkspaceQuery = userWorkspaceQuery.Where(x => x.WorkspaceId == workspaceId);
            }

            var userWorkspaces = await userWorkspaceQuery.ToListAsync();
            return userWorkspaces.ToArray();
        }

        public async Task<UserWorkspaceModel> GetUserWorkspace(int userWorkspaceId)
        {
            var userWorkspace = await _dbContext.UserWorkspaces
                .Select(x => new UserWorkspaceModel
                {
                    Id = x.Id,
                    DeskHeight = x.DeskHeight,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    UserName = x.User.UserName,
                    UserId = x.User.Id,
                    WorkspaceId = x.Workspace.Id,
                    WorkspaceNumber = x.Workspace.WorkspaceNumber,
                    RoomNumber = x.Workspace.RoomNumber
                })
                .FirstOrDefaultAsync(x => x.Id == userWorkspaceId);

            if (userWorkspace == null)
            {
                throw new NotFoundException($"Could not find user workspace for with id '{userWorkspaceId}'!");
            }

            return userWorkspace;
        }

        public async Task<UserWorkspaceModel> CreateUserWorkspace(SaveUserWorkspaceModel model)
        {
            var userWorkspace = _mapper.Map<UserWorkspace>(model);

            userWorkspace.Workspace = await _dbContext.Workspaces.FirstOrDefaultAsync(x => x.Id == model.WorkspaceId);
            userWorkspace.User = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            await _dbContext.UserWorkspaces.AddAsync(userWorkspace);

            await _dbContext.SaveChangesAsync();

            return await GetUserWorkspace(userWorkspace.Id);
        }

        public async Task<UserWorkspaceModel> UpdateUserWorkspace(SaveUserWorkspaceModel model)
        {
            var userWorkspace = await _dbContext.UserWorkspaces.FirstOrDefaultAsync(x => x.User.Id == model.UserId && x.Workspace.Id == model.WorkspaceId);

            if (userWorkspace == null)
            {
                throw new NotFoundException($"Could not find user workspace for user with id '{model.UserId}' and workspace with id '{model.WorkspaceId}'!");
            }

            _mapper.Map(model, userWorkspace);

            userWorkspace.Workspace = await _dbContext.Workspaces.FirstOrDefaultAsync(x => x.Id == model.WorkspaceId);
            userWorkspace.User = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            await _dbContext.SaveChangesAsync();

            return await GetUserWorkspace(userWorkspace.Id);
        }

        public async Task DeleteUserWorkspace(int userId, int workspaceId)
        {
            var userWorkspace = await _dbContext.UserWorkspaces.FirstOrDefaultAsync(x => x.User.Id == userId && x.Workspace.Id == workspaceId);

            if (userWorkspace == null)
            {
                throw new NotFoundException($"Could not find user workspace for user with id '{userId}' and workspace with id '{workspaceId}'!");
            }

            _dbContext.UserWorkspaces.Remove(userWorkspace);

            await _dbContext.SaveChangesAsync();
        }
    }
}
