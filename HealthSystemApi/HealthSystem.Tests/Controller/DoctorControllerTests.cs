using FakeItEasy;
using FluentAssertions;
using HealthSystem.Admins.Controllers;
using HealthSystem.Admins.Models;
using HealthSystem.Admins.Services.DoctorService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Tests.Controller
{
    public class DoctorControllerTests
    {
        private readonly IDoctorService doctorService;

        public DoctorControllerTests()
        {
            doctorService = A.Fake<IDoctorService>();
        }

        [Theory]
        [InlineData("Алерголог", "test", "nz", "088", "mario.19@abv.bg", "Mario Petkov", 2)]
        [InlineData("Гастроентеролог", "teееst", "nzзз", "088", "mario.19@abv.bg", "Ivailo Petkov", 3)]
        [InlineData("Кардиолог", "test", "nzззз", "087858", "mario.19@abv.bg", "Martin Petkov", 4)]
        [InlineData("Хирург", "teеst", "nззz", "0887758", "mario.19@abv.bg", "Dragan Petkov", 5)]
        public async void DoctorController_Add_ReturnOK(string specialization, string userId, string about, string contactNumber, string email, string FullName, int hospitalId)
        {
            //Arrange
            var model = new DoctorAddModel()
            {
                About = about,
                ContactNumber = contactNumber,
                Email = email,
                Specialization = specialization,
                UserId = userId,
                HospitalId = hospitalId,
                FullName = FullName
            };

            A.CallTo(() => doctorService.AddAsync(model, "test")).Returns(true);
            var controller = new DoctorController(doctorService);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test"),
                new Claim(ClaimTypes.Role, "Director")
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            //Act

            var result = await controller.Add(model);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Theory]
        [InlineData(null, "test", "nz", "088", "mario.19@abv.bg", "Mario Petkov", 2)]
        [InlineData("Гастроентеролог", null, "nzзз", "088", "mario.19@abv.bg", "Ivailo Petkov", 3)]
        [InlineData("Кардиолог", "test", "nzззз", null, "mario.19@abv.bg", "Martin Petkov", 4)]
        [InlineData("Хирург", "teеst", "nззz", "0887758", null, "Dragan Petkov", 5)]
        [InlineData("Хирург", "teеst", "nззz", "0887758", "mario.19@abv.bg", null, 5)]
        [InlineData("Хирург", "teеst", "nззz", "0887758", "mario.19@abv.bg", null, 6)]
        public async void DoctorController_Add_ReturnBadRequest(string specialization, string userId, string about, string contactNumber, string email, string FullName, int hospitalId)
        {
            //Arrange
            var model = new DoctorAddModel()
            {
                About = about,
                ContactNumber = contactNumber,
                Email = email,
                Specialization = specialization,
                UserId = userId,
                FullName = FullName
            };

            if (hospitalId != 6)
            {
                model.HospitalId = hospitalId;
            }

            A.CallTo(() => doctorService.AddAsync(model, "test")).Returns(false);
            var controller = new DoctorController(doctorService);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test"),
                new Claim(ClaimTypes.Role, "Director")
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            //Act

            var result = await controller.Add(model);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void DoctorController_Remove_ReturnOK(int id)
        {
            A.CallTo(() => doctorService.RemoveAsync(id, "test", "sample_token")).Returns(true);
            var controller = new DoctorController(doctorService);

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
            result.Should().BeOfType(typeof(OkResult));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void DoctorController_Remove_ReturnBadRequest(int id)
        {
            A.CallTo(() => doctorService.RemoveAsync(id, "test", null)).Returns(false);
            var controller = new DoctorController(doctorService);

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
                    Request = { Headers = { ["Authorization"] = "Bearerr sample_token" } }
                }
            };

            //Act

            var result = await controller.Remove(id);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestResult));
        }

        [Theory]
        [InlineData(4.36F, "test", 1, 2)]
        [InlineData(3.36F, "test", 2, 1)]
        [InlineData(5.00F, "test", 5, 4)]
        [InlineData(2.36F, "test", 6, 3)]
        [InlineData(4.56F, "test", 15, 5)]
        public async void DoctorController_AddRating_ReturnOK(float rating, string comment, int doctorId, int appointmentId)
        {
            A.CallTo(() => doctorService.AddRating(rating, comment, doctorId, appointmentId, "test")).Returns(true);
            var controller = new DoctorController(doctorService);

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
                    Request = { Headers = { ["Authorization"] = "Bearerr sample_token" } }
                }
            };

            //Act

            var result = await controller.AddRating(rating, comment, doctorId, appointmentId);

            //Assert

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
