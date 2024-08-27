using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FakeItEasy;
using FluentAssertions;
using HealthSystem.Identity.Controllers;
using HealthSystem.Identity.Data;
using HealthSystem.Identity.Data.Models;
using HealthSystem.Identity.Models;
using HealthSystem.Identity.Services.IdentityService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Tests.Controller
{
    public class AuthenticationControllerTests
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityService _identityService;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            // Mock DbContext
            var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            _context = new IdentityDbContext(options);

            // Mock UserManager
            _userManager = A.Fake<UserManager<User>>(x =>
                x.WithArgumentsForConstructor(
                    new object[]
                    {
                        A.Fake<IUserStore<User>>(),
                        null, null, null, null, null, null, null, null
                    }));

            // Mock SignInManager
            _signInManager = A.Fake<SignInManager<User>>(x =>
                x.WithArgumentsForConstructor(
                    new object[]
                    {
                        _userManager,
                        A.Fake<IHttpContextAccessor>(),
                        A.Fake<IUserClaimsPrincipalFactory<User>>(),
                        null, null, null, null
                    }));

            // Mock IdentityService
            _identityService = A.Fake<IIdentityService>();

            // Initialize Controller
            _controller = new AuthenticationController(_context, _userManager, _signInManager, _identityService);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var registerModel = new RegisterModel { Email = "test@test.com", Password = "Test@123", FullName = "Test User" };
            A.CallTo(() => _userManager.CreateAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(IdentityResult.Success);

            // Act
            var result = await _controller.Register(registerModel);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var registerModel = new RegisterModel { Email = "test@test.com", Password = "Test@123", FullName = "Test User" };
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Invalid password" });
            A.CallTo(() => _userManager.CreateAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(identityResult);

            // Act
            var result = await _controller.Register(registerModel);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().Be(identityResult);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenLoginFails()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "test@test.com", Password = "Test@123" };
            A.CallTo(() => _userManager.FindByEmailAsync(loginModel.Email)).Returns(Task.FromResult<User>(null));

            // Act
            var result = await _controller.Login(loginModel);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task SuperLogin_ReturnsBadRequest_WhenLoginFailsOrMultipleRoles()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "test@test.com", Password = "Test@123" };
            var user = new User { Email = loginModel.Email };
            var roles = new List<string> { "User", "Admin" };
            Microsoft.AspNetCore.Identity.SignInResult result =
            await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);
            A.CallTo(() => _userManager.FindByEmailAsync(loginModel.Email)).Returns(user);
            A.CallTo(() => _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false))
                .Returns(result);
            A.CallTo(() => _userManager.GetRolesAsync(user)).Returns(roles);

            // Act
            var res = await _controller.SuperLogin(loginModel);

            // Assert
            res.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task IsAuthenticated_ReturnsTrue_WhenUserIsAuthenticated()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user123")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.IsAuthenticated();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(new { IsAuthenticated = true, UserId = "user123" });
        }

        [Fact]
        public async Task IsAdmin_ReturnsTrue_WhenUserIsAdmin()
        {
            // Arrange
            var token = "valid-token";
            A.CallTo(() => _identityService.IsAdministrator(token)).Returns(true);

            // Act
            var result = await _controller.IsAdmin(token);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be(true);
        }

        [Fact]
        public async Task IsAdmin_ReturnsFalse_WhenUserIsNotAdmin()
        {
            // Arrange
            var token = "invalid-token";
            A.CallTo(() => _identityService.IsAdministrator(token)).Returns(false);

            // Act
            var result = await _controller.IsAdmin(token);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be(false);
        }

        [Fact]
        public async Task GetNameByUserId_ReturnsUserName_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = "1", FullName = "Test User" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetNameByUserId(user.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be(user.FullName);
        }
    }
}
