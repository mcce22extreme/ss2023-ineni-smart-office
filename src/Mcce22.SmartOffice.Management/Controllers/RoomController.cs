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

        /// <summary>
        /// Retrieve a list of available rooms.
        /// </summary>
        /// <response code="200">Rooms retrieved successfully.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to retrieve rooms.</response>
        [HttpGet]
        public async Task<RoomModel[]> GetRooms()
        {
            return await _roomManager.GetRooms();
        }

        /// <summary>
        /// Retrieve a specific room.
        /// </summary>
        /// <param name="roomId" example="1">The room id.</param>
        /// <returns>The room with the given id.</returns>
        /// <response code="200">Room retrieved successfully.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to retrieve rooms.</response>
        /// <response code="404">Room with the given id was not found.</response>
        [HttpGet("{roomId}")]
        public async Task<RoomModel> GetRoom(int roomId)
        {
            return await _roomManager.GetRoom(roomId);
        }

        /// <summary>
        /// Create a new room.
        /// </summary>
        /// <param name="model">Information of the new room.</param>
        /// <returns>The new created room.</returns>
        /// <response code="200">Room created successfully.</response>
        /// <response code="400">Validation error occured.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to create rooms.</response>
        [HttpPost]
        public async Task<RoomModel> CreateOperator([FromBody] SaveRoomModel model)
        {
            return await _roomManager.CreateRoom(model);
        }

        /// <summary>
        /// Update an existing room.
        /// </summary>
        /// <param name="roomId" example="1">The room id.</param>
        /// <param name="model">Information of the room.</param>
        /// <returns>The updated room.</returns>
        /// <response code="200">Room updated successfully.</response>
        /// <response code="400">Validation error occured.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to update room.</response>
        /// <response code="404">Room with the given id was not found.</response>
        [HttpPut("{roomId}")]
        public async Task<RoomModel> UpdateOperator(int roomId, [FromBody] SaveRoomModel model)
        {
            return await _roomManager.UpdateRoom(roomId, model);
        }

        /// <summary>
        /// Delete an existing room.
        /// </summary>
        /// <param name="roomId" example="1">The room id.</param>
        /// <response code="200">Room deleted successfully.</response>        
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to delete rooms.</response>
        /// <response code="404">Room with the given id was not found.</response>
        [HttpDelete("{roomId}")]
        public async Task DeleteRoom(int roomId)
        {
            await _roomManager.DeleteRoom(roomId);
        }
    }
}
