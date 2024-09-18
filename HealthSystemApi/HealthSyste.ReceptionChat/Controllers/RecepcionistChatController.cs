using Microsoft.AspNetCore.Mvc;
using HealthSystem.ReceptionChat.Hubs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HealthSyste.ReceptionChat.Services.RecepcionistService;

namespace HealthSyste.ReceptionChat.Controllers
{
    /// <summary>
    /// Controller to manage receptionist-related chat functionalities, including sending and retrieving messages and rooms.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RecepcionistChatController : ControllerBase
    {
        private readonly IRecepcionistService _recepcionistService;
        private readonly ChatHub _chatHub;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecepcionistChatController"/> class.
        /// </summary>
        /// <param name="recepcionistService">Service to manage receptionist-specific logic.</param>
        /// <param name="chatHub">Hub to manage real-time chat functionality.</param>
        public RecepcionistChatController(IRecepcionistService recepcionistService, ChatHub chatHub)
        {
            _recepcionistService = recepcionistService;
            _chatHub = chatHub;
        }

        /// <summary>
        /// Retrieves the list of chat rooms available to the currently logged-in receptionist.
        /// </summary>
        /// <returns>A list of rooms associated with the receptionist.</returns>
        [HttpGet("GetMyRooms")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> GetMyRooms()
        {
            // Retrieves the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Calls service to get rooms for the receptionist
            var rooms = await _recepcionistService.GetMyRooms(userId ?? "invalid");

            // Returns the list of rooms in the response
            return Ok(new { Rooms = rooms });
        }

        /// <summary>
        /// Sends a message to a specific chat room.
        /// </summary>
        /// <param name="model">The model containing the room name, message, and sender's name.</param>
        /// <returns>An OK response if the message is successfully sent, otherwise a BadRequest.</returns>
        [HttpPost("SendMessageToRoom")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> SendMessageToRoom([FromBody] SendMessageModel model)
        {
            // Validates that the room name and message are not empty
            if (string.IsNullOrEmpty(model.RoomName) || string.IsNullOrEmpty(model.Message))
            {
                return BadRequest("Invalid room name or message.");
            }

            // Sends the message to the specified room via the ChatHub
            await _chatHub.SendMessageToRoom(model.RoomName, model.Message, model.Name ?? "Unknown");

            // Returns OK status if message was sent successfully
            return Ok();
        }

        /// <summary>
        /// Retrieves all messages from a specific chat room.
        /// </summary>
        /// <param name="roomName">The name of the room to retrieve messages from.</param>
        /// <returns>A list of messages from the specified room.</returns>
        [HttpGet("GetRoomMessages")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> GetRoomMessages([FromQuery] string roomName)
        {
            var messages = await _recepcionistService.GetRoomMessages(roomName);

            var result = new List<MessageDisplayModel>();

            foreach (var message in messages)
            {
                result.Add(new MessageDisplayModel
                {
                    SentByUserName = message.Item2, 
                    Message = message.Item1         
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Deletes a specific chat room within the context of a hospital.
        /// </summary>
        /// <param name="roomName">The name of the room to be deleted.</param>
        /// <param name="hospitalId">The ID of the hospital associated with the room.</param>
        /// <returns>An OK response if the room was successfully deleted.</returns>
        [HttpGet("DeleteRoom")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> DeleteRoom([FromQuery] string roomName, int hospitalId)
        {
            await _chatHub.DeleteRoom(hospitalId, roomName);

            return Ok();
        }
    }

    /// <summary>
    /// Model representing the data required to send a message to a chat room.
    /// </summary>
    public class SendMessageModel
    {
        /// <summary>
        /// Gets or sets the name of the chat room.
        /// </summary>
        public string? RoomName { get; set; }

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the name of the user sending the message.
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// Model representing a displayable message with the sender's name.
    /// </summary>
    public class MessageDisplayModel
    {
        /// <summary>
        /// Gets or sets the name of the user who sent the message.
        /// </summary>
        public string? SentByUserName { get; set; }

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        public string? Message { get; set; }
    }
}
