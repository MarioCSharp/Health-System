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

        public async Task Edit(DoctorDetailsModel model)
        {
            var doc = await context.DoctorsInfo.FirstOrDefaultAsync(x => x.DoctorId == model.Id);
            var doctor = await context.Doctors.FindAsync(model.Id);

            doctor.Specialization = model.Specialization;
            await context.SaveChangesAsync();
            doc.Email = model.Email;
            doc.About = model.About;
            doc.ContactNumber = model.ContactNumber;
            doc.FullName = model.FullName;
            doc.Specialization = model.Specialization;
            await context.SaveChangesAsync();
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

        public async Task<DoctorAddModel> GetDoctor(int id)
        {
            var info = await context.Doctors.FindAsync(id);

            return new DoctorAddModel()
            {
                HospitalId = info.HospitalId,
                Specialization = info.Specialization,
                UserId = info.UserId
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
