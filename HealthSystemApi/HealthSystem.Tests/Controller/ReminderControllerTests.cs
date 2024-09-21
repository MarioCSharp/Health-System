using FakeItEasy;
using FluentAssertions;
using HealthSystem.Documents.Controllers;
using HealthSystem.Documents.Models;
using HealthSystem.Documents.Services.ReminderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Tests.Controller
{
    public class ReminderControllerTests
    {
        private readonly IReminderService reminderService;
        private readonly ReminderController controller;

        public ReminderControllerTests()
        {
            reminderService = A.Fake<IReminderService>();
            controller = new ReminderController(reminderService);
        }

        [Fact]
        public async Task ReminderController_Add_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var model = new ReminderAddModel { /* Initialize valid properties */ };
            var userId = "TestUserId";
            A.CallTo(() => reminderService.AddAsync(model, userId)).Returns(true);

            var httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuthentication"));
            httpContext.User = user;
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.Add(model);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            A.CallTo(() => reminderService.AddAsync(model, userId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ReminderController_Add_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            controller.ModelState.AddModelError("Error", "Invalid model");

            var model = new ReminderAddModel();

            // Act
            var result = await controller.Add(model);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be(model);

            A.CallTo(() => reminderService.AddAsync(A<ReminderAddModel>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ReminderController_Add_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var model = new ReminderAddModel { /* Initialize valid properties */ };
            var userId = "TestUserId";
            A.CallTo(() => reminderService.AddAsync(model, userId)).Returns(false);

            var httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuthentication"));
            httpContext.User = user;
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.Add(model);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            A.CallTo(() => reminderService.AddAsync(model, userId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ReminderController_Remove_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var reminderId = 1;
            var userId = "TestUserId";
            A.CallTo(() => reminderService.RemoveAsync(reminderId, userId)).Returns(true);

            var httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuthentication"));
            httpContext.User = user;
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.Remove(reminderId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            A.CallTo(() => reminderService.RemoveAsync(reminderId, userId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ReminderController_Remove_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var reminderId = 1;
            var userId = "TestUserId";
            A.CallTo(() => reminderService.RemoveAsync(reminderId, userId)).Returns(false);

            var httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuthentication"));
            httpContext.User = user;
            controller.ControllerContext.HttpContext = httpContext;

            // Act
            var result = await controller.Remove(reminderId);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            A.CallTo(() => reminderService.RemoveAsync(reminderId, userId)).MustHaveHappenedOnceExactly();
        }
    }
}
