using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Users.Entities;
using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IDynamoDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;

        public UserManager(IDynamoDBContext dbContext, IMapper mapper, IIdGenerator idGenerator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<UserModel[]> GetUsers()
        {
            var users = await _dbContext
                .ScanAsync<User>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            return users
                .OrderBy(x => x.UserName)
                .Select(_mapper.Map<UserModel>)
                .ToArray();
        }

        public async Task<UserModel> GetUser(string userId)
        {
            var user = await _dbContext.LoadAsync<User>(userId);

            return user == null ? throw new NotFoundException($"Could not find user with id '{userId}'!") : _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUser(SaveUserModel model)
        {
            var user = _mapper.Map<User>(model);

            user.Id = _idGenerator.GenerateId();

            await _dbContext.SaveAsync(user);

            return await GetUser(user.Id);
        }

        public async Task<UserModel> UpdateUser(string userId, SaveUserModel model)
        {
            var user = await _dbContext.LoadAsync<User>(userId) ?? throw new NotFoundException($"Could not find user with id '{userId}'!");

            _mapper.Map(model, user);

            await _dbContext.SaveAsync(user);

            return await GetUser(userId);
        }

        public async Task DeleteUser(string userId)
        {
            await _dbContext.DeleteAsync<User>(userId);
        }
    }
}
