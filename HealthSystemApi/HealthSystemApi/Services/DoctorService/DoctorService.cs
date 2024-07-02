﻿using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Doctor;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private ApplicationDbContext context;
        public DoctorService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(DoctorAddModel model)
        {
            var doctor = new Doctor()
            {
                Specialization = model.Specialization,
                HospitalId = model.HospitalId,
                UserId = model.UserId,
            };

            await context.Doctors.AddAsync(doctor);
            await context.SaveChangesAsync();

            return await context.Doctors.ContainsAsync(doctor);
        }

        public async Task<List<DoctorModel>> GetAllAsync(int id)
        {
            return await context.Doctors.Where(x => x.HospitalId == id)
                .Select(x => new DoctorModel
                {
                    Id = x.Id,
                    FullName = x.User.FullName,
                    Specialization = x.Specialization
                }).ToListAsync();
        }

        public async Task<DoctorDetailsModel> GetDetailsAsync(int id)
        {
            var info = await context.DoctorsInfo.FindAsync(id);

            return new DoctorDetailsModel
            {
                Id = info.Id,
                FullName = info.FullName,
                Specialization = info.Specialization,
                ContactNumber = info.ContactNumber,
                Email = info.Email,
                About = info.About
            };
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var doctor = await context.Doctors.FindAsync(id);

            if (doctor is null) return false;

            context.Doctors.Remove(doctor);
            await context.SaveChangesAsync(true);

            return !await context.Doctors.ContainsAsync(doctor);
        }
    }
}
