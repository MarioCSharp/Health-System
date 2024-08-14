using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private ApplicationDbContext context;

        private string _secretKey = "MedCare?Authentication?Secret?Token";  // You shouldn`t store this here!
        private string _issuer = "http://localhost:5166";
        private string _audience = "http://localhost:5166";

        public AuthenticationController(UserManager<User> userManager,
                                        SignInManager<User> signInManager,
                                        ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.context = context;
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

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                string token = GenerateToken(user.Id);

                return Ok(new { Token = token });
            }

            return BadRequest(result);
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);

                if (result.Succeeded)
                {
                    string token = GenerateToken(user.Id);

                    return Ok(new { Token = token });
                }
            }

            return BadRequest();
        }

        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromQuery] string token)
        {
            //TODO: Fix logout
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet("IsAuthenticated")]
        public async Task<IActionResult> IsAuthenticated([FromQuery] string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            if (valid)
            {
                var t = new JwtSecurityToken(token);

                return Ok(new { IsAuthenticated = valid, UserId = t.Subject });
            }

            return Ok(new { IsAuthenticated = false }); ;
        }

        [HttpGet("IsAdmin")]
        public async Task<IActionResult> IsAdmin([FromQuery] string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            if (!valid)
            {
                return Ok(false);
            }

            var t = new JwtSecurityToken(token);

            var user = await context.Users.FindAsync(t.Subject);

            var isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

            return Ok(isAdmin);
        }

        private string GenerateToken(string userId)
        {
            var tokenExpiration = DateTime.UtcNow.AddDays(7);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;

            var ticks = long.Parse(tokenExp);

            return ticks;
        }
    }
}
