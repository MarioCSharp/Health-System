using FakeItEasy;
using FluentAssertions;
using HealthSystem.Admins.Controllers;
using HealthSystem.Admins.Services.RecepcionistService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Tests.Controller
{
    public class RecepcionistControllerTests
    {
        private readonly IRecepcionistService recepcionistService;
        private readonly RecepcionistController controller;

        public RecepcionistControllerTests()
        {
            recepcionistService = A.Fake<IRecepcionistService>();
            controller = new RecepcionistController(recepcionistService);
        }

        [Fact]
        public async Task Add_ShouldReturnOk_WhenTokenIsValid()
        {
            // Arrange
            var userId = "User123";
            var hospitalId = 1;
            var name = "John Doe";
            var token = "valid_token";

            A.CallTo(() => recepcionistService.AddAsync(userId, hospitalId, name, token))
                .Returns(Task.FromResult(true));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer " + token;
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.Add(userId, hospitalId, name);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            A.CallTo(() => recepcionistService.AddAsync(userId, hospitalId, name, token))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Add_ShouldReturnBadRequest_WhenTokenIsMissing()
        {
            // Arrange
            var userId = "User123";
            var hospitalId = 1;
            var name = "John Doe";

            var httpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.Add(userId, hospitalId, name);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();
        }

        [Fact]
        public async Task GetHospitalId_ShouldReturnOkWithHospitalId()
        {
            // Arrange
            var userId = "User123";
            var hospitalId = 1;

            A.CallTo(() => recepcionistService.GetHospitalIdAsync(userId))
                .Returns(Task.FromResult(hospitalId));

            // Act
            var result = await controller.GetHospitalId(userId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(hospitalId);

            A.CallTo(() => recepcionistService.GetHospitalIdAsync(userId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetHospitalAndUserId_ShouldReturnOkWithUserAndHospitalId()
        {
            // Arrange
            var userId = "User123";
            var hospitalId = 1;

            var httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuthentication"));

            httpContext.User = user;
            controller.ControllerContext.HttpContext = httpContext;

            A.CallTo(() => recepcionistService.GetHospitalIdAsync(userId))
                .Returns(Task.FromResult(hospitalId));

            // Act
            var result = await controller.GetHospitalAndUserId();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(new { UserId = userId, HospitalId = hospitalId });

            A.CallTo(() => recepcionistService.GetHospitalIdAsync(userId))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenTokenIsValid()
        {
            // Arrange
            var id = 1;
            var token = "valid_token";

            A.CallTo(() => recepcionistService.Delete(id, token)).Returns(Task.CompletedTask);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer " + token;
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.Delete(id);

            // Assert
            var okResult = result as OkResult;
            okResult.Should().NotBeNull();

            A.CallTo(() => recepcionistService.Delete(id, token))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenTokenIsMissing()
        {
            // Arrange
            var id = 1;

            var httpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.Delete(id);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();
        }
    }
}
