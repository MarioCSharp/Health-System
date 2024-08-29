using HealthSyste.ReceptionChat.Services.RecepcionistService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSyste.ReceptionChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecepcionistChatController : ControllerBase
    {
        private IRecepcionistService recepcionistService;

        public RecepcionistChatController(IRecepcionistService recepcionistService)
        {
            this.recepcionistService = recepcionistService;
        }

        [HttpGet("GetMyRooms")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> GetMyRooms()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var rooms = await recepcionistService.GetMyRooms(userId ?? "invalid");

            return Ok(new { Rooms = rooms });
        }
    }
}
