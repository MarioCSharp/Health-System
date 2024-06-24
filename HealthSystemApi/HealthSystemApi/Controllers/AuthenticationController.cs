using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        public AuthenticationController(UserManager<User> userManager,
                                        SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet("Register")]
        public async Task<IActionResult> Register([FromQuery] RegisterModel registerModel)
        {
            var user = new User
            {
                Email = registerModel.Email,
                FullName = registerModel.FullName,
                UserName = registerModel.Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                return Ok();
            }

            return BadRequest(result);
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] LoginModel loginModel)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);

                if (result.Succeeded)
                {
                    return Ok(result);
                }
            }

            return BadRequest();
        }

        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }
    }
}
