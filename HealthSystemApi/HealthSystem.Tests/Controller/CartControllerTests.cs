using FakeItEasy;
using FluentAssertions;
using HealthSystem.Pharmacy.Controllers;
using HealthSystem.Pharmacy.Models.Cart;
using HealthSystem.Pharmacy.Services.CartService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace HealthSystem.Tests.Controller
{
    public class CartControllerTests
    {
        private readonly ICartService cartService;
        private readonly CartController controller;

        public CartControllerTests()
        {
            cartService = A.Fake<ICartService>();
            controller = new CartController(cartService);
        }

        [Fact]
        public async Task CartController_AddToCart_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var model = new AddToCartModel { /* Initialize valid properties */ };
            A.CallTo(() => cartService.AddToCartAsync(model)).Returns(true);

            // Act
            var result = await controller.AddToCart(model);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            A.CallTo(() => cartService.AddToCartAsync(model)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CartController_AddToCart_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var model = new AddToCartModel { /* Initialize valid properties */ };
            A.CallTo(() => cartService.AddToCartAsync(model)).Returns(false);

            // Act
            var result = await controller.AddToCart(model);

            // Assert
            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();

            A.CallTo(() => cartService.AddToCartAsync(model)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CartController_RemoveFromCart_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var cartItemId = 1;
            A.CallTo(() => cartService.RemoveFromCartAsync(cartItemId)).Returns(true);

            // Act
            var result = await controller.RemoveFromCart(cartItemId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(true);

            A.CallTo(() => cartService.RemoveFromCartAsync(cartItemId)).MustHaveHappenedOnceExactly();
        }
    }
}
