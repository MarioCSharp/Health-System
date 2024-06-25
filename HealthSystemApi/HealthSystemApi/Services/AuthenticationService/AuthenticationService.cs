using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HealthSystemApi.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor httpContext;
        public AuthenticationService(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }
        public string GetUserId()
        {
            return httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
