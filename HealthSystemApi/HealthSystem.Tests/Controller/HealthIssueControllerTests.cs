using FakeItEasy;
using FluentAssertions;
using HealthSystem.HealthCare.Controllers;
using HealthSystem.HealthCare.Models;
using HealthSystem.HealthCare.Services.HealthIssueService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Tests.Controller
{
    public class HealthIssueControllerTests
    {
        private readonly IHealthIssueService healthIssueService;
        private readonly HealthIssueController controller;

        public HealthIssueControllerTests()
        {
            healthIssueService = A.Fake<IHealthIssueService>();
            controller = new HealthIssueController(healthIssueService);
        }

        [Fact]
        public async Task HealthIssueController_Add_ReturnsBadRequest_WhenStartDateInvalid()
        {
            // Arrange
            string startDate = "invalid-date";
            string endDate = "2024-08-27";
            string name = "Health Issue";
            string description = "Description";
            string userId = "user123";

            // Act
            var result = await controller.Add(startDate, endDate, name, description, userId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be("Invalid start date format. Please use yyyy-MM-dd.");
        }

        [Fact]
        public async Task HealthIssueController_Add_ReturnsBadRequest_WhenEndDateInvalid()
        {
            // Arrange
            string startDate = "2024-08-27";
            string endDate = "invalid-date";
            string name = "Health Issue";
            string description = "Description";
            string userId = "user123";

            // Act
            var result = await controller.Add(startDate, endDate, name, description, userId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be("Invalid end date format. Please use yyyy-MM-dd.");
        }

        [Fact]
        public async Task HealthIssueController_Remove_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            int id = 1;
            A.CallTo(() => healthIssueService.RemoveAsync(id)).Returns(false);

            // Act
            var result = await controller.Remove(id);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            // Verify that the service method was called exactly once
            A.CallTo(() => healthIssueService.RemoveAsync(id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HealthIssueController_Remove_ReturnsOk_WhenServiceSucceeds()
        {
            // Arrange
            int id = 1;
            A.CallTo(() => healthIssueService.RemoveAsync(id)).Returns(true);

            // Act
            var result = await controller.Remove(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            // Verify that the service method was called exactly once
            A.CallTo(() => healthIssueService.RemoveAsync(id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HealthIssueController_Details_ReturnsOk_WithHealthIssueDetails()
        {
            // Arrange
            int id = 1;
            var healthIssueDetails = new HealthIssueDetailsModel { Id = id, Name = "Test Issue" };
            A.CallTo(() => healthIssueService.DetailsAsync(id)).Returns(healthIssueDetails);

            // Act
            var result = await controller.Details(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(healthIssueDetails);

            // Verify that the service method was called exactly once
            A.CallTo(() => healthIssueService.DetailsAsync(id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task HealthIssueController_Edit_ReturnsOk_WhenServiceSucceeds()
        {
            // Arrange
            var editModel = new HealthIssueEditModel();
            A.CallTo(() => healthIssueService.EditAsync(editModel)).Returns(true);

            // Act
            var result = await controller.Edit(editModel);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            // Verify that the service method was called exactly once
            A.CallTo(() => healthIssueService.EditAsync(editModel)).MustHaveHappenedOnceExactly();
        }
    }
}
