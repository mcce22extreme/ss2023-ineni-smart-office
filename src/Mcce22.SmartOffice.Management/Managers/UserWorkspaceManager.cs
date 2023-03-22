using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management.Managers
{
    public interface IUserWorkspaceManager
    {
        Task<UserWorkspaceModel> GetUserWorkspace(int userId, int workspaceId);

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

        public async Task<UserWorkspaceModel> GetUserWorkspace(int userId, int workspaceId)
        {
            var userWorkspace = await _dbContext.UserWorkspaces.FirstOrDefaultAsync(x => x.UserId == userId && x.WorkspaceId == workspaceId);

            if (userWorkspace == null)
            {
                throw new NotFoundException($"Could not find user workspace for user with id '{userId}' and workspace with id '{workspaceId}'!");
            }

            return _mapper.Map<UserWorkspaceModel>(userWorkspace);
        }

        public async Task<UserWorkspaceModel> CreateUserWorkspace(SaveUserWorkspaceModel model)
        {
            var userWorkspace = _mapper.Map<UserWorkspace>(model);

            await _dbContext.UserWorkspaces.AddAsync(userWorkspace);

            await _dbContext.SaveChangesAsync();

            return await GetUserWorkspace(userWorkspace.UserId, userWorkspace.WorkspaceId);
        }

        public async Task<UserWorkspaceModel> UpdateUserWorkspace(SaveUserWorkspaceModel model)
        {
            var userWorkspace = await _dbContext.UserWorkspaces.FirstOrDefaultAsync(x => x.UserId == model.UserId && x.WorkspaceId == model.WorkspaceId);

            if (userWorkspace == null)
            {
                throw new NotFoundException($"Could not find user workspace for user with id '{model.UserId}' and workspace with id '{model.WorkspaceId}'!");
            }

            _mapper.Map(model, userWorkspace);

            await _dbContext.SaveChangesAsync();

            return await GetUserWorkspace(userWorkspace.UserId, userWorkspace.WorkspaceId);
        }

        public async Task DeleteUserWorkspace(int userId, int workspaceId)
        {
            var userWorkspace = await _dbContext.UserWorkspaces.FirstOrDefaultAsync(x => x.UserId == userId && x.WorkspaceId == workspaceId);

            if (userWorkspace == null)
            {
                throw new NotFoundException($"Could not find user workspace for user with id '{userId}' and workspace with id '{workspaceId}'!");
            }

            _dbContext.UserWorkspaces.Remove(userWorkspace);

            await _dbContext.SaveChangesAsync();
        }
    }
}
