using FakeItEasy;
using FluentAssertions;
using HealthSystem.Admins.Controllers;
using HealthSystem.Admins.Data.Models;
using HealthSystem.Admins.Models;
using HealthSystem.Admins.Services.DoctorService;
using HealthSystem.Admins.Services.HospitalService;
using MassTransit.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Tests.Controller
{
    public class HospitalControllerTests
    {
        private IHospitalService hospitalService;

        public HospitalControllerTests()
        {
            hospitalService = A.Fake<IHospitalService>();
        }

        //[Theory]
        //[InlineData("Test", "Pleven", "07888", "test")]
        //[InlineData("Test123", "Sofia", "07888", "test")]
        //[InlineData("Света Мария", "Пловдив", "07888", "test")]
        //public async void HospitalController_Add_ReturnOk(string hospitalName, string location, string contactNumber, string ownerId)
        //{
        //    //Arrange
        //    var model = new HospitalAddModel()
        //    {
        //        ContactNumber = contactNumber,
        //        HospitalName = hospitalName,
        //        Location = location,
        //        OwnerId = ownerId
        //    };

        //    A.CallTo(() => hospitalService.AddAsync(model)).Returns(true);
        //    var controller = new HospitalController(hospitalService);

        //    var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, "test"),
        //        new Claim(ClaimTypes.Role, "Director")
        //    }, "mock"));

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = user }
        //    };

        //    //Act

        //    var result = await controller.Add(model);

        //    //Assert

        //    result.Should().NotBeNull();
        //    result.Should().BeOfType(typeof(OkObjectResult));
        //}

        //[Theory]
        //[InlineData("Test", "Pleven", "07888", "test")]
        //[InlineData("Test123", "Sofia", "07888", "test")]
        //[InlineData("Света Мария", "Пловдив", "07888", "test")]
        //public async void HospitalController_Add_ReturnBadRequest(string hospitalName, string location, string contactNumber, string ownerId)
        //{
        //    //Arrange
        //    var model = new HospitalAddModel()
        //    {
        //        ContactNumber = contactNumber,
        //        HospitalName = hospitalName,
        //        Location = location,
        //        OwnerId = ownerId
        //    };

        //    A.CallTo(() => hospitalService.AddAsync(model)).Returns(false);
        //    var controller = new HospitalController(hospitalService);

        //    var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, "test"),
        //        new Claim(ClaimTypes.Role, "Director")
        //    }, "mock"));

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = user }
        //    };

        //    //Act

        //    var result = await controller.Add(model);

        //    //Assert

        //    result.Should().NotBeNull();
        //    result.Should().BeOfType(typeof(BadRequestResult));
        //}

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async void HospitalController_Remove_ReturnOK(int id)
        {
            //Arrange
            A.CallTo(() => hospitalService.RemoveAsync(id, "sample_token")).Returns(true);
            var controller = new HospitalController(hospitalService);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test"),
                new Claim(ClaimTypes.Role, "Director")
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user,
                    Request = { Headers = { ["Authorization"] = "Bearer sample_token" } }
                }
            };

            //Act

            var result = await controller.Remove(id);

            //Assert

            result.Should().NotBeNull();

            result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
