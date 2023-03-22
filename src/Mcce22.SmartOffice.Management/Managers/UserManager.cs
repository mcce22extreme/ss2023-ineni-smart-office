using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management.Managers
{
    public interface IUserManager
    {
        Task<UserModel[]> GetUsers();

        Task<UserModel> GetUser(int userId);

        Task<UserModel> CreateUser(SaveUserModel model);

        Task<UserModel> UpdateUser(int userId, SaveUserModel model);

        Task DeleteUser(int userId);
    }

    public class UserManager : IUserManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserManager(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserModel[]> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();

            return users.Select(_mapper.Map<UserModel>).ToArray();
        }

        public async Task<UserModel> GetUser(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"Could not find user with id '{userId}'!");
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUser(SaveUserModel model)
        {
            var user = _mapper.Map<User>(model);

            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();

            return await GetUser(user.Id);
        }

        public async Task<UserModel> UpdateUser(int userId, SaveUserModel model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"Could not find user with id '{userId}'!");
            }

            _mapper.Map(model, user);

            await _dbContext.SaveChangesAsync();

            return await GetUser(userId);
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"Could not find user with id '{userId}'!");
            }

            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();
        }
    }
}
