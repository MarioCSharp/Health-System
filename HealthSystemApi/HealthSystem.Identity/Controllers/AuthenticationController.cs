using HealthSystem.Identity.Data;
using HealthSystem.Identity.Data.Models;
using HealthSystem.Identity.Services.IdentityService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HealthSystem.Identity.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace HealthSystem.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IdentityDbContext context;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private IIdentityService authenticationService;

        private string _secretKey = "MedCare?Authentication?Secret?Token";  // You shouldn`t store this here!
        private string _issuer = "http://localhost:5166";
        private string _audience = "http://localhost:5166";

        public AuthenticationController(IdentityDbContext context,
                                        UserManager<User> userManager,
                                        SignInManager<User> signInManager,
                                        IIdentityService authenticationService)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authenticationService = authenticationService;
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

                string token = await GenerateToken(user.Id);

                return Ok(new { Token = token });
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
                    string token = await GenerateToken(user.Id);

                    return Ok(new { Token = token });
                }
            }

            return BadRequest();
        }

        [HttpGet("SuperLogin")]
        public async Task<IActionResult> SuperLogin([FromQuery] LoginModel loginModel)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);

                var userRoles = await userManager.GetRolesAsync(user);

                if (result.Succeeded && userRoles.Count == 1)
                {
                    string token = await GenerateToken(user.Id);

                    return Ok(new { Token = token, Role = userRoles[0] });
                }
            }

            return BadRequest();
        }

        [HttpGet("IsAuthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { IsAuthenticated = true, UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
            }

            return Ok(new { IsAuthenticated = false });
        }

        [HttpGet("SecureIsAuthenticated")]
        public async Task<IActionResult> SecureIsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { IsAuthenticated = true, UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
            }

            return Ok(new { IsAuthenticated = false });
        }

        [HttpGet("IsAdmin")]
        public async Task<IActionResult> IsAdmin([FromQuery] string token)
        {
            return Ok(await authenticationService.IsAdministrator(token));
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await context.Users.Take(5).ToListAsync();

            return Ok(new
            {
                Users = users.Select(x => new UserDisplayModel()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email
                })
            });
        }

        [HttpGet("GetNameByUserId")]
        public async Task<IActionResult> GetNameByUserId([FromQuery] string userId)
        {
            var user = await context.Users.FindAsync(userId);

            return Ok(user.FullName ?? "Unknown");
        }

        [HttpGet("PutToRole")]
        [Authorize(Roles = "Administrator,Director,PharmacyOwner")]
        public async Task<IActionResult> PutToRole([FromQuery] string userId, string role)
        {
            var user = await context.Users.FindAsync(userId);

            if (user is null)
            {
                return BadRequest();
            }

            await userManager.AddToRoleAsync(user, role);

            return Ok();
        }

        [HttpGet("DeleteFromRole")]
        [Authorize(Roles = "Administrator,Director,PharmacyOwner")]
        public async Task<IActionResult> DeleteFromRole([FromQuery] string userId, string role)
        {
            var user = await context.Users.FindAsync(userId);

            await userManager.RemoveFromRoleAsync(user, role);

            return Ok();
        }

        private async Task<string> GenerateToken(string userId)
        {
            var tokenExpiration = DateTime.UtcNow.AddDays(7);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var user = await context.Users.FindAsync(userId);

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
