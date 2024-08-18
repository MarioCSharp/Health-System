using HealthSystem.Identity.Data;
using HealthSystem.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HealthSystem.Identity.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly IdentityDbContext context;
        private readonly UserManager<User> userManager;

        public IdentityService(IHttpContextAccessor httpContext,
                                     IdentityDbContext context,
                                     UserManager<User> userManager)
        {
            this.context = context;
            this.httpContext = httpContext;
            this.userManager = userManager;
        }

        public string GetUserId()
        {
            return httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<bool> RemoveUser(string userId)
        {
            //var user = await context.Users.FindAsync(userId);

            //if (user == null || await userManager.IsInRoleAsync(user, "Administrator"))
            //{
            //    return false;
            //}

            //var userMedications = context.Medications.Where(m => m.UserId == userId);
            //context.Medications.RemoveRange(userMedications);

            //var userSchedules = context.MedicationSchedules.Where(m => m.UserId == userId);
            //context.MedicationSchedules.RemoveRange(userSchedules);

            //var userBookings = context.Bookings.Where(b => b.UserId == userId);
            //context.Bookings.RemoveRange(userBookings);

            //var d = context.Doctors.Where(d => d.UserId == userId);
            //context.Doctors.RemoveRange(d);

            //var documents = context.Documents.Where(d => d.UserId == userId);
            //context.Documents.RemoveRange(documents);

            //var hI = context.HealthIssues.Where(d => d.UserId == userId);
            //context.HealthIssues.RemoveRange(hI);

            //var logs = context.Logs.Where(d => d.UserId == userId);
            //context.Logs.RemoveRange(logs);

            //var problems = context.Problems.Where(d => d.UserId == userId);
            //context.Problems.RemoveRange(problems);

            //context.Users.Remove(user);

            //await context.SaveChangesAsync();

            return true;
        }


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

        private bool TokenIsValid(string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }

        private long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;

            var ticks = long.Parse(tokenExp);

            return ticks;
        }

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
