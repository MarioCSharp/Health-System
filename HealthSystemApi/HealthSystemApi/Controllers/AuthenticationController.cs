﻿using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Authentication;
using HealthSystemApi.Services.AuthenticationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private IAuthenticationService authenticationService;

        private string _secretKey = "MedCare?Authentication?Secret?Token";  // You shouldn`t store this here!
        private string _issuer = "http://localhost:5166";
        private string _audience = "http://localhost:5166";

        public AuthenticationController(UserManager<User> userManager,
                                        SignInManager<User> signInManager,
                                        ApplicationDbContext context,
                                        IAuthenticationService authenticationService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.context = context;
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

        [HttpGet("SuperLogin")]
        public async Task<IActionResult> SuperLogin([FromQuery] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);

                var userRoles = await _userManager.GetRolesAsync(user);

                if (result.Succeeded && userRoles.Count == 1)
                {
                    string token = GenerateToken(user.Id);

                    return Ok(new { Token = token, Role = userRoles[0] });
                }
            }

            return BadRequest();
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

        [HttpGet("SecureIsAuthenticated")]
        public async Task<IActionResult> SecureIsAuthenticated([FromQuery] string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            var t = new JwtSecurityToken(token);

            if (valid)
            {
                return Ok(new { IsAuthenticated = valid, UserId = t.Subject });
            }

            return Ok(new { IsAuthenticated = false }); ;
        }

        [HttpGet("IsAdmin")]
        public async Task<IActionResult> IsAdmin([FromQuery] string token)
        {
            return Ok(await authenticationService.IsAdministrator(token));
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return Ok(new { Users = new List<UserDisplayModel>() });
            }

            var users = await context.Users.Take(5).ToListAsync();

            return Ok(new {Users = users.Select(x => new UserDisplayModel()
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email
            })});
        }

        [HttpGet("RemoveUser")]
        public async Task<IActionResult> RemoveUser([FromQuery] string userId, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return Ok(new { Success = false });
            }

            var result = await authenticationService.RemoveUser(userId);

            return Ok(new { Success = result });
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
