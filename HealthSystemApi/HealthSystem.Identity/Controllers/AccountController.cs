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

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("GetAccountsWithNoRoles")]
        [Authorize(Roles = "Administrator,Director,PharmacyOwner")]
        public async Task<IActionResult> GetAccountsWithNoRoles()
        {
            var users = await accountService.GetAccountsAsync();

            return Ok(new { Users = users });
        }
    }
}
