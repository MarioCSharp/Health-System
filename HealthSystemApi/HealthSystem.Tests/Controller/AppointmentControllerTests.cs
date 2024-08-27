using System.Security.Claims;
using FakeItEasy;
using FluentAssertions;
using HealthSystem.Booking.Controllers;
using HealthSystem.Booking.Models;
using HealthSystem.Booking.Services.AppointmentService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Tests.Controller
{
    public class AppointmentControllerTests
    {
        private readonly IAppointmentService appointmentService;
        private readonly AppointmentController controller;

        public AppointmentControllerTests()
        {
            appointmentService = A.Fake<IAppointmentService>();
            controller = new AppointmentController(appointmentService);
        }

        [Theory]
        [InlineData("John Doe", "01/01/1980", "1234567890", "123 Main St", "Headache", "Migraine", "Normal", "Active", "Medication", "Blood Test", "Dr. Smith", "UIN123", "sample_token", 1)]
        public async Task AppointmentController_IssuePrescription_ReturnOK(
                string fullName,
                string dateOfBirth,
                string egn,
                string address,
                string complaints,
                string diagnosis,
                string conditions,
                string status,
                string therapy,
                string tests,
                string doctorName,
                string uin,
                string? token,
                int appointmentId)
        {
            var userId = "TestUserId";
            var fakeFile = A.Fake<IFormFile>();
            A.CallTo(() => fakeFile.OpenReadStream()).Returns(new MemoryStream());
            A.CallTo(() => fakeFile.ContentType).Returns("application/pdf");
            A.CallTo(() => fakeFile.FileName).Returns("prescription.pdf");

            var model = new PrescriptionModel
            {
                FullName = fullName,
                DateOfBirth = dateOfBirth,
                EGN = egn,
                Address = address,
                Complaints = complaints,
                Diagnosis = diagnosis,
                Conditions = conditions,
                Status = status,
                Therapy = therapy,
                Tests = tests,
                DoctorName = doctorName,
                UIN = uin,
                Token = token,
                AppointmentId = appointmentId
            };

            A.CallTo(() => appointmentService.IssuePrescriptionAsync(model, userId))
                .Returns(Task.FromResult<(bool, IFormFile?)>((true, fakeFile)));

            var httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuthentication"));

            httpContext.User = user;
            controller.ControllerContext.HttpContext = httpContext;

            var result = await controller.IssuePrescription(model);

            var fileResult = result as FileStreamResult;
            fileResult.Should().NotBeNull();
            fileResult.ContentType.Should().Be("application/pdf");
            fileResult.FileDownloadName.Should().Be("prescription.pdf");

            // Verifying that the service was called exactly once with the right parameters
            A.CallTo(() => appointmentService.IssuePrescriptionAsync(model, userId)).MustHaveHappenedOnceExactly();
        }

        [Theory]
        [InlineData("John Doe", "01/01/1980", "1234567890", "123 Main St", "Headache", "Migraine", "Normal", "Active", "Medication", "Blood Test", "Dr. Smith", "UIN123", "sample_token", 1)]
        public async Task AppointmentController_IssuePrescription_ReturnBadRequest(
                string fullName,
                string dateOfBirth,
                string egn,
                string address,
                string complaints,
                string diagnosis,
                string conditions,
                string status,
                string therapy,
                string tests,
                string doctorName,
                string uin,
                string? token,
                int appointmentId)
        {
            var userId = "TestUserId";
            var model = new PrescriptionModel
            {
                FullName = fullName,
                DateOfBirth = dateOfBirth,
                EGN = egn,
                Address = address,
                Complaints = complaints,
                Diagnosis = diagnosis,
                Conditions = conditions,
                Status = status,
                Therapy = therapy,
                Tests = tests,
                DoctorName = doctorName,
                UIN = uin,
                Token = token,
                AppointmentId = appointmentId
            };

            A.CallTo(() => appointmentService.IssuePrescriptionAsync(model, userId))
                .Returns(Task.FromResult<(bool, IFormFile?)>((false, null)));

            var httpContext = new DefaultHttpContext();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuthentication"));

            httpContext.User = user;
            controller.ControllerContext.HttpContext = httpContext;

            var result = await controller.IssuePrescription(model);

            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be("Failed to issue prescription");

            A.CallTo(() => appointmentService.IssuePrescriptionAsync(model, userId)).MustHaveHappenedOnceExactly();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task AppointmentController_HasPrescription_ReturnOK(int appointmentId)
        {
            // Arrange
            var fakeFile = A.Fake<IFormFile>();
            A.CallTo(() => fakeFile.OpenReadStream()).Returns(new MemoryStream());
            A.CallTo(() => fakeFile.ContentType).Returns("application/pdf");
            A.CallTo(() => fakeFile.FileName).Returns("prescription.pdf");

            // Specify the type of the tuple explicitly
            A.CallTo(() => appointmentService.HasPrescriptionAsync(appointmentId))
                .Returns(Task.FromResult<(bool, IFormFile?)>((true, fakeFile)));

            // Act
            var result = await controller.HasPrescription(appointmentId);

            // Assert
            var fileResult = result as FileStreamResult;
            fileResult.Should().NotBeNull();
            fileResult.ContentType.Should().Be("application/pdf");
            fileResult.FileDownloadName.Should().Be("prescription.pdf");

            // Verify that the service was called exactly once with the right parameter
            A.CallTo(() => appointmentService.HasPrescriptionAsync(appointmentId)).MustHaveHappenedOnceExactly();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task AppointmentController_HasPrescription_ReturnBadRequest(int appointmentId)
        {
            // Arrange
            // Specify the type of the tuple explicitly
            A.CallTo(() => appointmentService.HasPrescriptionAsync(appointmentId))
                .Returns(Task.FromResult<(bool, IFormFile?)>((false, null)));

            // Act
            var result = await controller.HasPrescription(appointmentId);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            // Verify that the service was called exactly once with the right parameter
            A.CallTo(() => appointmentService.HasPrescriptionAsync(appointmentId)).MustHaveHappenedOnceExactly();
        }
    }
}
