using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Users.Entities;
using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Managers
{
    public interface IUserManager
    {
        Task<UserModel[]> GetUsers();

        Task<UserModel> GetUser(string userId);

        Task<UserModel> CreateUser(SaveUserModel model);

        Task<UserModel> UpdateUser(string userId, SaveUserModel model);

        Task DeleteUser(string userId);
    }

    public class UserManager : IUserManager
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;

        public UserManager(IAmazonDynamoDB dynamoDbClient, IMapper mapper, IIdGenerator idGenerator)
        {
            _dynamoDbClient = dynamoDbClient;
            _mapper = mapper;
            _idGenerator = idGenerator;
        }

        public async Task<UserModel[]> GetUsers()
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var users = await context
                .ScanAsync<User>(Array.Empty<ScanCondition>())
                .GetRemainingAsync();

            return users
                .OrderBy(x => x.UserName)
                .Select(_mapper.Map<UserModel>)
                .ToArray();
        }

        public async Task<UserModel> GetUser(string userId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var user = await context.LoadAsync<User>(userId);

            if (user == null)
            {
                throw new NotFoundException($"Could not find user with id '{userId}'!");
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUser(SaveUserModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var user = _mapper.Map<User>(model);

            user.Id = _idGenerator.GenerateId();

            await context.SaveAsync(user);

            return await GetUser(user.Id);
        }

        public async Task<UserModel> UpdateUser(string userId, SaveUserModel model)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var user = await context.LoadAsync<User>(userId);

            if (user == null)
            {
                throw new NotFoundException($"Could not find user with id '{userId}'!");
            }

            _mapper.Map(model, user);

            await context.SaveAsync(user);

            return await GetUser(userId);
        }

        public async Task DeleteUser(string userId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            await context.DeleteAsync<User>(userId);
        }
    }
}
