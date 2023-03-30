using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController
    {
        private readonly IRoomManager _roomManager;

        public RoomController(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        [HttpGet]
        public async Task<RoomModel[]> GetRooms()
        {
            return await _roomManager.GetRooms();
        }

        [HttpGet("{roomId}")]
        public async Task<RoomModel> GetRoom(int roomId)
        {
            return await _roomManager.GetRoom(roomId);
        }

        [HttpPost]
        public async Task<RoomModel> CreateOperator([FromBody] SaveRoomModel model)
        {
            return await _roomManager.CreateRoom(model);
        }

        [HttpPut("{roomId}")]
        public async Task<RoomModel> UpdateOperator(int roomId, [FromBody] SaveRoomModel model)
        {
            return await _roomManager.UpdateRoom(roomId, model);
        }

        [HttpDelete("{roomId}")]
        public async Task DeleteRoom(int roomId)
        {
            await _roomManager.DeleteRoom(roomId);
        }
    }
}
