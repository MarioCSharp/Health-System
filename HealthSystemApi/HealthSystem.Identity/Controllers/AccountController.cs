using HealthSystem.Identity.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Identity.Controllers
{
    /// <summary>
    /// API controller for managing accounts in the Health System.
    /// </summary>
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="accountService">The account service to manage account-related operations.</param>
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Retrieves all accounts that do not have any assigned roles.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a list of users without roles.</returns>
        [HttpGet("GetAccountsWithNoRoles")]
        [Authorize(Roles = "Administrator,Director,PharmacyOwner")]
        public async Task<IActionResult> GetAccountsWithNoRoles()
        {
            var users = await accountService.GetAccountsAsync();

            return Ok(new { Users = users });
        }
    }
}
