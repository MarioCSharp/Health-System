using HealthSystemApi.Services.AccountService;
using HealthSystemApi.Services.AuthenticationService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystemApi.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService accountService;
        private IAuthenticationService authenticationService;

        public AccountController(IAccountService accountService,
                                 IAuthenticationService authenticationService)
        {
            this.accountService = accountService;
            this.authenticationService = authenticationService;
        }

        [HttpGet("GetUserAppointments")]
        public async Task<IActionResult> GetUserAppointments([FromQuery] string userId, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return BadRequest();
            }

            var result = await accountService.GetAppointments(userId);

            return Ok(new { FullName = result.Item1, Appointments = result.Item2 });
        }
    }
}
