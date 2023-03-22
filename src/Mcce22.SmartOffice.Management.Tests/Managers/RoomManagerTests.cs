using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Models;
using NUnit.Framework;

namespace Mcce22.SmartOffice.Management.Tests.Managers
{
    [TestFixture]
    public class RoomManagerTests : TestBase
    {
        private async Task<Room> CreateRoom()
        {
            var room = new Room
            {
                Number = Make.String()
            };

            using var dbContext = CreateDbContext();

            await dbContext.Rooms.AddAsync(room);
            await dbContext.SaveChangesAsync();

            return room;
        }

        [Test]
        public async Task GetRooms_ReturnsRooms()
        {
            var expected = await CreateRoom();

            var manager = new RoomManager(CreateDbContext(), Mapper);

            var rooms = await manager.GetRooms();

            Assert.IsTrue(rooms.Length == 1);

            var actual = rooms.FirstOrDefault();

            Assert.IsNotNull(actual);
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Number, Is.EqualTo(expected.Number));
        }

        [Test]
        public async Task GetRoom_ReturnsRoom()
        {
            var expected = await CreateRoom();

            var manager = new RoomManager(CreateDbContext(), Mapper);

            var actual = await manager.GetRoom(expected.Id);

            Assert.IsNotNull(actual);
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Number, Is.EqualTo(expected.Number));
        }

        [Test]
        public async Task CreateRoom_CreatesNewRoom()
        {
            var expected = new SaveRoomModel
            {
                Number = Make.String(),
            };

            var manager = new RoomManager(CreateDbContext(), Mapper);

            var actual = await manager.CreateRoom(expected);

            Assert.IsNotNull(actual);
            Assert.That(actual.Id, Is.GreaterThan(0));
            Assert.That(actual.Number, Is.EqualTo(expected.Number));
        }        
    }
}
