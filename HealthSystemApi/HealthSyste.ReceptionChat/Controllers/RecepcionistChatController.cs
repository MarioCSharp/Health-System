using Microsoft.AspNetCore.Mvc;
using HealthSystem.ReceptionChat.Hubs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HealthSyste.ReceptionChat.Services.RecepcionistService;

namespace HealthSyste.ReceptionChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecepcionistChatController : ControllerBase
    {
        private readonly IRecepcionistService recepcionistService;
        private readonly ChatHub _chatHub;

        public RecepcionistChatController(IRecepcionistService recepcionistService, ChatHub chatHub)
        {
            this.recepcionistService = recepcionistService;
            _chatHub = chatHub;
        }

        [HttpGet("GetMyRooms")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> GetMyRooms()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var rooms = await recepcionistService.GetMyRooms(userId ?? "invalid");

            return Ok(new { Rooms = rooms });
        }

        [HttpPost("SendMessageToRoom")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> SendMessageToRoom([FromBody] SendMessageModel model)
        {
            if (string.IsNullOrEmpty(model.RoomName) || string.IsNullOrEmpty(model.Message))
            {
                return BadRequest("Invalid room name or message");
            }

            await _chatHub.SendMessageToRoom(model.RoomName, model.Message, model.Name ?? "Unknown");

            return Ok();
        }

        [HttpGet("GetRoomMessages")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> GetRoomMessages([FromQuery] string roomName)
        {
            var messages = await recepcionistService.GetRoomMessages(roomName);

            var result = new List<MessageDisplayModel>();

            foreach (var message in messages) 
            {
                result.Add(new MessageDisplayModel()
                {
                    SentByUserName = message.Item2,
                    Message = message.Item1
                });
            }

            return Ok(result);
        }
    }

    public class SendMessageModel
    {
        public string? RoomName { get; set; }
        public string? Message { get; set; }
        public string? Name { get; set; }
    }

    public class MessageDisplayModel
    {
        public string? SentByUserName { get; set; }
        public string? Message { get; set; }
    }
}
