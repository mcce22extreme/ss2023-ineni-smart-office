using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management.Managers
{
    public interface IRoomManager
    {
        Task<RoomModel[]> GetRooms();

        Task<RoomModel> GetRoom(int roomId);

        Task<RoomModel> CreateRoom(SaveRoomModel model);

        Task<RoomModel> UpdateRoom(int roomId, SaveRoomModel model);

        Task DeleteRoom(int roomId);
    }

    public class RoomManager : IRoomManager
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public RoomManager(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RoomModel[]> GetRooms()
        {
            var operators = await _dbContext.Rooms.ToListAsync();

            return operators.Select(_mapper.Map<RoomModel>).ToArray();
        }

        public async Task<RoomModel> GetRoom(int roomId)
        {
            var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);

            if (room == null)
            {
                throw new NotFoundException($"Could not find room with id '{roomId}'!");
            }

            return _mapper.Map<RoomModel>(room);
        }

        public async Task<RoomModel> CreateRoom(SaveRoomModel model)
        {
            var room = _mapper.Map<Room>(model);

            await _dbContext.Rooms.AddAsync(room);

            await _dbContext.SaveChangesAsync();

            return await GetRoom(room.Id);
        }

        public async Task<RoomModel> UpdateRoom(int roomId, SaveRoomModel model)
        {
            var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);

            if (room == null)
            {
                throw new NotFoundException($"Could not find room with id '{roomId}'!");
            }

            _mapper.Map(model, room);

            await _dbContext.SaveChangesAsync();

            return await GetRoom(roomId);
        }

        public async Task DeleteRoom(int roomId)
        {
            var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);

            if (room == null)
            {
                throw new NotFoundException($"Could not find room with id '{roomId}'!");
            }

            _dbContext.Rooms.Remove(room);

            await _dbContext.SaveChangesAsync();
        }
    }
}
