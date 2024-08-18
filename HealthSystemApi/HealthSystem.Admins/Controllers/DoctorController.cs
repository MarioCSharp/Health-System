﻿using HealthSystem.Admins.Models;
using HealthSystem.Admins.Services.DoctorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Admins.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private IDoctorService doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        [HttpGet("Add")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Add([FromQuery] DoctorAddModel model)
        {
            var result = await doctorService.AddAsync(model);

            return result ? Ok(new { Success = result }) : BadRequest();
        }

        [HttpGet("Remove")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await doctorService.RemoveAsync(id);

            return result ? Ok() : BadRequest();
        }

        [HttpGet("All")]
        public async Task<IActionResult> All([FromQuery] int id)
        {
            var result = await doctorService.GetAllAsync(id);

            return Ok(result);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await doctorService.GetDetailsAsync(id));
        }

        [HttpGet("GetDoctor")]
        public async Task<IActionResult> GetDoctor([FromQuery] int id)
        {
            return Ok(await doctorService.GetDoctor(id));
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] DoctorDetailsModel model)
        {
            await doctorService.Edit(model);

            return Ok();
        }

        [HttpGet("GetDoctorByUserId")]
        public async Task<IActionResult> GetDoctorByUserId(string userId)
        {
            return Ok(await doctorService.GetDoctorByUserId(userId));
        }
    }
}
