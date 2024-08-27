using FakeItEasy;
using FluentAssertions;
using HealthSystem.Booking.Controllers;
using HealthSystem.Booking.Models;
using HealthSystem.Booking.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Tests.Controller
{
    public class ServiceControllerTests
    {
        private readonly IServiceService serviceService;
        private readonly ServiceController controller;

        public ServiceControllerTests()
        {
            serviceService = A.Fake<IServiceService>();
            controller = new ServiceController(serviceService);
        }

        [Fact]
        public async Task ServiceController_Add_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange: Create a model with an invalid state
            var model = new ServiceAddModel();
            controller.ModelState.AddModelError("Name", "Required");

            // Act: Call the Add method
            var result = await controller.Add(model);

            // Assert: Check that the result is a BadRequestObjectResult with the model state
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();

            // Check that the value of the BadRequestObjectResult is the ModelStateDictionary
            var modelStateDictionary = badRequestResult.Value as SerializableError;
            modelStateDictionary.Should().NotBeNull();

            // Verify that the error contains the expected key and error message
            modelStateDictionary.ContainsKey("Name").Should().BeTrue();
            modelStateDictionary["Name"].Should().BeAssignableTo<string[]>().Which.Should().ContainSingle()
                .Which.Should().Be("Required");

            // Verify that the service method was not called
            A.CallTo(() => serviceService.AddAsync(A<ServiceAddModel>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ServiceController_Add_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange: Create a valid model
            var model = new ServiceAddModel { Name = "Test Service" };

            // Arrange: Configure the service to return false
            A.CallTo(() => serviceService.AddAsync(model)).Returns(false);

            // Act: Call the Add method
            var result = await controller.Add(model);

            // Assert: Check that the result is a BadRequestResult
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            // Verify that the service method was called exactly once
            A.CallTo(() => serviceService.AddAsync(model)).MustHaveHappenedOnceExactly();
        }
    }
}
