using FakeItEasy;
using FluentAssertions;
using HealthSystem.Documents.Controllers;
using HealthSystem.Documents.Models;
using HealthSystem.Documents.Services.DocumentService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Tests.Controller
{
    public class DocumentControllerTests
    {
        private readonly IDocumentService documentService;
        private readonly DocumentController controller;

        public DocumentControllerTests()
        {
            documentService = A.Fake<IDocumentService>();
            controller = new DocumentController(documentService);
        }

        [Fact]
        public async Task DocumentController_Add_ReturnsBadRequest_WhenFileIsNull()
        {
            // Arrange
            var model = new DocumentAddModel();

            // Act
            var result = await controller.Add(model, null);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be("File is empty.");
        }

        [Fact]
        public async Task DocumentController_Add_ReturnsBadRequest_WhenFileIsEmpty()
        {
            // Arrange
            var model = new DocumentAddModel();
            var emptyFile = A.Fake<IFormFile>();
            A.CallTo(() => emptyFile.Length).Returns(0);

            // Act
            var result = await controller.Add(model, emptyFile);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be("File is empty.");
        }

        [Fact]
        public async Task DocumentController_Add_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new DocumentAddModel();
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.Length).Returns(100);
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await controller.Add(model, file);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be("Invalid model state");

            // Verify that the service method was not called
            A.CallTo(() => documentService.AddAsync(A<DocumentAddModel>._, A<string>._, A<IFormFile>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task DocumentController_Add_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var model = new DocumentAddModel { UserId = "user123" };
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.Length).Returns(100);
            A.CallTo(() => documentService.AddAsync(model, model.UserId, file)).Returns(false);

            // Act
            var result = await controller.Add(model, file);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            // Verify that the service method was called exactly once
            A.CallTo(() => documentService.AddAsync(model, model.UserId, file)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DocumentController_Add_ReturnsOk_WhenServiceSucceeds()
        {
            // Arrange
            var model = new DocumentAddModel { UserId = "user123" };
            var file = A.Fake<IFormFile>();
            A.CallTo(() => file.Length).Returns(100);
            A.CallTo(() => documentService.AddAsync(model, model.UserId, file)).Returns(true);

            // Act
            var result = await controller.Add(model, file);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            // Verify that the service method was called exactly once
            A.CallTo(() => documentService.AddAsync(model, model.UserId, file)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DocumentController_Remove_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var documentId = 1;
            A.CallTo(() => documentService.RemoveAsync(documentId)).Returns(false);

            // Act
            var result = await controller.Remove(documentId);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            // Verify that the service method was called exactly once
            A.CallTo(() => documentService.RemoveAsync(documentId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DocumentController_Remove_ReturnsOk_WhenServiceSucceeds()
        {
            // Arrange
            var documentId = 1;
            A.CallTo(() => documentService.RemoveAsync(documentId)).Returns(true);

            // Act
            var result = await controller.Remove(documentId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            // Verify that the service method was called exactly once
            A.CallTo(() => documentService.RemoveAsync(documentId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DocumentController_Edit_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var model = new DocumentEditModel();
            A.CallTo(() => documentService.EditAsync(model)).Returns(false);

            // Act
            var result = await controller.Edit(model);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            // Verify that the service method was called exactly once
            A.CallTo(() => documentService.EditAsync(model)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DocumentController_Edit_ReturnsOk_WhenServiceSucceeds()
        {
            // Arrange
            var model = new DocumentEditModel();
            A.CallTo(() => documentService.EditAsync(model)).Returns(true);

            // Act
            var result = await controller.Edit(model);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            // Verify that the service method was called exactly once
            A.CallTo(() => documentService.EditAsync(model)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DocumentController_Details_ReturnsOk_WithDocumentDetails()
        {
            // Arrange
            var documentId = 1;
            var documentDetails = new DocumentDetailsModel { Id = documentId, Title = "Test Document" };
            A.CallTo(() => documentService.DetailsAsync(documentId)).Returns(documentDetails);

            // Act
            var result = await controller.Details(documentId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(documentDetails);

            // Verify that the service method was called exactly once
            A.CallTo(() => documentService.DetailsAsync(documentId)).MustHaveHappenedOnceExactly();
        }
    }
}
