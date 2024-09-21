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
    /// <summary>
    /// Controller for handling authentication and user management operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IdentityDbContext context;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private IIdentityService authenticationService;

        private string _secretKey = "MedCare?Authentication?Secret?Token";  // You shouldn't store this here!
        private string _issuer = "http://localhost:5166";
        private string _audience = "http://localhost:5166";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="context">The Identity database context.</param>
        /// <param name="userManager">The user manager to handle user operations.</param>
        /// <param name="signInManager">The sign-in manager to handle login operations.</param>
        /// <param name="authenticationService">The identity service to handle authentication.</param>
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

        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="registerModel">The model containing registration details.</param>
        /// <returns>Returns a JWT token if registration is successful.</returns>
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

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="loginModel">The model containing login details.</param>
        /// <returns>Returns a JWT token if login is successful.</returns>
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

        /// <summary>
        /// Logs in a user and returns their role if only one role is assigned.
        /// </summary>
        /// <param name="loginModel">The model containing login details.</param>
        /// <returns>Returns a JWT token and the user's role if successful.</returns>
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

        /// <summary>
        /// Checks if the current user is authenticated.
        /// </summary>
        /// <returns>Returns true if the user is authenticated.</returns>
        [HttpGet("IsAuthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { IsAuthenticated = true, UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
            }

            return Ok(new { IsAuthenticated = false });
        }

        /// <summary>
        /// Securely checks if the current user is authenticated.
        /// </summary>
        /// <returns>Returns true if the user is authenticated.</returns>
        [HttpGet("SecureIsAuthenticated")]
        public async Task<IActionResult> SecureIsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { IsAuthenticated = true, UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
            }

            return Ok(new { IsAuthenticated = false });
        }

        /// <summary>
        /// Checks if the provided token belongs to an administrator.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>Returns true if the token belongs to an administrator.</returns>
        [HttpGet("IsAdmin")]
        public async Task<IActionResult> IsAdmin([FromQuery] string token)
        {
            return Ok(await authenticationService.IsAdministrator(token));
        }

        /// <summary>
        /// Retrieves a list of all users, restricted to administrators.
        /// </summary>
        /// <returns>Returns a list of user display models.</returns>
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

        /// <summary>
        /// Retrieves the full name of a user based on their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns the user's full name, or "Unknown" if not found.</returns>
        [HttpGet("GetNameByUserId")]
        public async Task<IActionResult> GetNameByUserId([FromQuery] string userId)
        {
            var user = await context.Users.FindAsync(userId);

            return Ok(user.FullName ?? "Unknown");
        }

        /// <summary>
        /// Adds a user to a specified role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="role">The role to assign.</param>
        /// <returns>Returns a success result if the operation is successful.</returns>
        [HttpGet("PutToRole")]
        [Authorize(Roles = "Administrator,Director,PharmacyOwner")]
        public async Task<IActionResult> PutToRole([FromQuery] string userId, string role)
        {
            var containsRole = await context.Roles.AnyAsync(x => x.Name == role);

            if (!containsRole)
            {
                var newRole = new IdentityRole(role);

                await roleManager.CreateAsync(newRole);
            }

            var user = await context.Users.FindAsync(userId);

            if (user is null)
            {
                return BadRequest();
            }

            await userManager.AddToRoleAsync(user, role);

            return Ok();
        }

        /// <summary>
        /// Removes a user from a specified role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="role">The role to remove.</param>
        /// <returns>Returns a success result if the operation is successful.</returns>
        [HttpGet("DeleteFromRole")]
        [Authorize(Roles = "Administrator,Director,PharmacyOwner")]
        public async Task<IActionResult> DeleteFromRole([FromQuery] string userId, string role)
        {
            var user = await context.Users.FindAsync(userId);

            await userManager.RemoveFromRoleAsync(user, role);

            return Ok();
        }

        /// <summary>
        /// Generates a JWT token for the provided user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>Returns a JWT token as a string.</returns>
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

        /// <summary>
        /// Gets the expiration time of a JWT token.
        /// </summary>
        /// <param name="token">The JWT token to check.</param>
        /// <returns>Returns the expiration time in ticks as a long value.</returns>
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
