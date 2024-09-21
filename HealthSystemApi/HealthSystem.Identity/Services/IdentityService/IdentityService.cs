using HealthSystem.Identity.Data;
using HealthSystem.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HealthSystem.Identity.Services.IdentityService
{
    /// <summary>
    /// Service responsible for identity-related operations such as user role checking and token validation.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IdentityDbContext context;
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityService"/> class.
        /// </summary>
        /// <param name="httpContext">The <see cref="IHttpContextAccessor"/> used to access the current HTTP context.</param>
        /// <param name="context">The <see cref="IdentityDbContext"/> to access user data.</param>
        /// <param name="userManager">The <see cref="UserManager{User}"/> used to manage user roles.</param>
        public IdentityService(IHttpContextAccessor httpContext,
                                     IdentityDbContext context,
                                     UserManager<User> userManager)
        {
            this.context = context;
            this.httpContext = httpContext;
            this.userManager = userManager;
        }

        /// <summary>
        /// Retrieves the user ID from the current HTTP context.
        /// </summary>
        /// <returns>The user ID as a string, or null if not found.</returns>
        public string GetUserId()
        {
            return httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// Checks if the user associated with the given token is an administrator.
        /// </summary>
        /// <param name="token">The JWT token to validate and extract user information from.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The task result contains a boolean value indicating whether the user is an administrator.</returns>
        public async Task<bool> IsAdministrator(string token)
        {
            var isValid = TokenIsValid(token);

            if (!isValid)
            {
                return false;
            }

            var t = new JwtSecurityToken(token);

            var userId = t.Subject;

            var user = await context.Users.FindAsync(userId);

            return await userManager.IsInRoleAsync(user, "Administrator");
        }

        /// <summary>
        /// Validates if the given token is still valid by checking its expiration time.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>True if the token is valid, otherwise false.</returns>
        private bool TokenIsValid(string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }

        /// <summary>
        /// Retrieves the expiration time of the given token.
        /// </summary>
        /// <param name="token">The JWT token to extract the expiration time from.</param>
        /// <returns>The expiration time as a long value representing Unix time.</returns>
        private long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;

            var ticks = long.Parse(tokenExp);

            return ticks;
        }

        /// <summary>
        /// Checks if the user associated with the given token is a doctor.
        /// </summary>
        /// <param name="token">The JWT token to validate and extract user information from.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The task result contains a boolean value indicating whether the user is a doctor.</returns>
        public async Task<bool> IsDoctor(string token)
        {
            var isValid = TokenIsValid(token);

            if (!isValid)
            {
                return false;
            }

            var t = new JwtSecurityToken(token);

            var userId = t.Subject;

            var user = await context.Users.FindAsync(userId);

            return await userManager.IsInRoleAsync(user, "Doctor");
        }

        /// <summary>
        /// Checks if the user associated with the given token is a director.
        /// </summary>
        /// <param name="token">The JWT token to validate and extract user information from.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. 
        /// The task result contains a boolean value indicating whether the user is a director.</returns>
        public async Task<bool> IsDirector(string token)
        {
            var isValid = TokenIsValid(token);

            if (!isValid)
            {
                return false;
            }

            var t = new JwtSecurityToken(token);

            var userId = t.Subject;

            var user = await context.Users.FindAsync(userId);

            return await userManager.IsInRoleAsync(user, "Director");
        }
    }
}
