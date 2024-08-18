using HealthSystem.Identity.Services.AccountService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Identity.Controllers
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

        [HttpGet("GetAccountsWithNoRoles")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetAccountsWithNoRoles()
        {
            var users = await accountService.GetAccountsAsync();

            return Ok(new { Users = users });
        }
    }
}
