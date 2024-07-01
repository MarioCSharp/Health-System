using HealthSystemApi.Data;
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
