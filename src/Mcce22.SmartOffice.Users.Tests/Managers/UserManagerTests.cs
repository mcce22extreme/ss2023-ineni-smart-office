using Amazon.DynamoDBv2.DataModel;
using FakeItEasy;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Users.Entities;
using Mcce22.SmartOffice.Users.Managers;
using Mcce22.SmartOffice.Users.Models;
using NUnit.Framework;

namespace Mcce22.SmartOffice.Users.Tests.Managers
{
    [TestFixture]
    public class UserManagerTests : TestBase
    {
        private User CreateUser()
        {
            return new User
            {
                Id = IdGenerator.GenerateId(),
                FirstName = Make.String(),
                LastName = Make.String(),
                Email = Make.String(),
                UserName = Make.String()
            };
        }

        private List<User> CreateUsers()
        {
            var users = new List<User>();

            for (int i = 0; i < 10; i++)
            {
                users.Add(CreateUser());
            }

            return users;
        }

        private SaveUserModel CreateSaveUserModel()
        {
            return new SaveUserModel
            {
                FirstName = Make.String(),
                LastName = Make.String(),
                Email = Make.String(),
                UserName = Make.String(),
            };
        }

        [Test]
        public async Task GetUsers_ReturnsUsers()
        {
            // Arrange
            var expectedUsers = CreateUsers();
            var dbContext = A.Fake<IDynamoDBContext>();
            var result = A.Fake<AsyncSearch<User>>();

            A.CallTo(() => result.GetRemainingAsync(A<CancellationToken>.Ignored)).Returns(Task.FromResult(expectedUsers));
            A.CallTo(() => dbContext.ScanAsync<User>(Array.Empty<ScanCondition>(), A<DynamoDBOperationConfig>.Ignored))
                .Returns(result);

            // Act
            var manager = new UserManager(dbContext, Mapper, IdGenerator);
            var otherUsers = await manager.GetUsers();

            // Assert
            foreach (var expectedUser in expectedUsers)
            {
                var otherUser = otherUsers.FirstOrDefault(x => x.Id == expectedUser.Id);

                Assert.IsNotNull(otherUser);
                Assert.That(otherUser.FirstName, Is.EqualTo(expectedUser.FirstName));
                Assert.That(otherUser.LastName, Is.EqualTo(expectedUser.LastName));
                Assert.That(otherUser.Email, Is.EqualTo(expectedUser.Email));
                Assert.That(otherUser.UserName, Is.EqualTo(expectedUser.UserName));
            }
        }

        [Test]
        public async Task GetUser_ReturnsExistingUser()
        {
            // Arrange
            var expectedUser = CreateUser();
            var dbContext = A.Fake<IDynamoDBContext>();

            A.CallTo(() => dbContext.LoadAsync<User>(expectedUser.Id, A<CancellationToken>.Ignored))
                .Returns(expectedUser);

            // Act
            var manager = new UserManager(dbContext, Mapper, IdGenerator);
            var otherUser = await manager.GetUser(expectedUser.Id);

            // Assert
            Assert.IsNotNull(otherUser);
            Assert.That(otherUser.FirstName, Is.EqualTo(expectedUser.FirstName));
            Assert.That(otherUser.LastName, Is.EqualTo(expectedUser.LastName));
            Assert.That(otherUser.Email, Is.EqualTo(expectedUser.Email));
            Assert.That(otherUser.UserName, Is.EqualTo(expectedUser.UserName));
        }

        [Test]
        public void GetUser_ForUnknownUser_ThrowsException()
        {
            // Arrange
            var dbContext = A.Fake<IDynamoDBContext>();
            var userId = Make.String(12);

            A.CallTo(() => dbContext.LoadAsync<User>(userId, A<CancellationToken>.Ignored)).Returns((User)null);

            // Act
            var manager = new UserManager(dbContext, Mapper, IdGenerator);

            // Assert
            Assert.ThrowsAsync<NotFoundException>(() => manager.GetUser(userId));
        }

        [Test]
        public async Task CreateUser_MapsAllInformation()
        {
            // Arrange            
            var dbContext = A.Fake<IDynamoDBContext>();
            var saveUser = CreateSaveUserModel();

            // Act
            var manager = new UserManager(dbContext, Mapper, IdGenerator);
            await manager.CreateUser(saveUser);

            // Assert
            A.CallTo(() => dbContext.SaveAsync(A<User>.That.Matches(otherUser =>
                otherUser.FirstName.Equals(saveUser.FirstName) &&
                otherUser.LastName.Equals(saveUser.LastName) &&
                otherUser.Email.Equals(saveUser.Email) &&
                otherUser.UserName.Equals(saveUser.UserName)
                ),
                A<CancellationToken>.Ignored))
                .MustHaveHappened();
        }

        [Test]
        public async Task Createuser_UsesIdGenerator()
        {
            // Arrange            
            var dbContext = A.Fake<IDynamoDBContext>();
            var idGenerator = A.Fake<IIdGenerator>();
            var userId = Make.String(12);
            var saveUser = CreateSaveUserModel();

            A.CallTo(() => idGenerator.GenerateId(12)).Returns(userId);

            // Act
            var manager = new UserManager(dbContext, Mapper, idGenerator);
            await manager.CreateUser(saveUser);

            // Assert
            A.CallTo(() => dbContext.SaveAsync(A<User>.That.Matches(otherUser =>
                otherUser.Id.Equals(userId)),
                A<CancellationToken>.Ignored))
                .MustHaveHappened();
        }

        [Test]
        public async Task UpdateUser_MapsAllInformation()
        {
            // Arrange            
            var dbContext = A.Fake<IDynamoDBContext>();
            var expectedUser = CreateUser();
            var saveUser = CreateSaveUserModel();

            A.CallTo(() => dbContext.LoadAsync<User>(expectedUser.Id, A<CancellationToken>.Ignored))
                .Returns(expectedUser);

            // Act
            var manager = new UserManager(dbContext, Mapper, IdGenerator);
            await manager.UpdateUser(expectedUser.Id, saveUser);

            // Assert
            A.CallTo(() => dbContext.SaveAsync(A<User>.That.Matches(otherUser =>
                otherUser.FirstName.Equals(saveUser.FirstName) &&
                otherUser.LastName.Equals(saveUser.LastName) &&
                otherUser.Email.Equals(saveUser.Email) &&
                otherUser.UserName.Equals(saveUser.UserName)
                ),
                A<CancellationToken>.Ignored))
                .MustHaveHappened();
        }

        [Test]
        public void UpdateUser_ForUnknownUser_ThrowsException()
        {
            // Arrange            
            var dbContext = A.Fake<IDynamoDBContext>();
            var userId = Make.String(12);
            var saveUser = CreateSaveUserModel();

            A.CallTo(() => dbContext.LoadAsync<User>(userId, A<CancellationToken>.Ignored)).Returns((User)null);

            // Act
            var manager = new UserManager(dbContext, Mapper, IdGenerator);

            // Assert
            Assert.ThrowsAsync<NotFoundException>(() => manager.UpdateUser(userId, saveUser));
        }

        [Test]
        public async Task DeleteUsers_CallsContextDelete()
        {
            // Arrange            
            var dbContext = A.Fake<IDynamoDBContext>();
            var userId = Make.String(12);

            // Act
            var manager = new UserManager(dbContext, Mapper, IdGenerator);
            await manager.DeleteUser(userId);

            // Assert
            A.CallTo(() => dbContext.DeleteAsync<User>(userId, A<CancellationToken>.Ignored)).MustHaveHappened();
        }
    }
}
