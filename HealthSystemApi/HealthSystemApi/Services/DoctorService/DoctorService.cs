using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Doctor;
using HealthSystemApi.Models.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private ApplicationDbContext context;
        private UserManager<User> userManager;

        public DoctorService(ApplicationDbContext context,
                             UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<bool> AddAsync(DoctorAddModel model)
        {
            var user = await context.Users.FindAsync(model.UserId);

            if (user == null)
            {
                return false;
            }

            var doctor = new Doctor()
            {
                Specialization = model.Specialization,
                HospitalId = model.HospitalId,
                UserId = model.UserId,
            };

            await context.Doctors.AddAsync(doctor);
            await context.SaveChangesAsync();

            var doctorInfo = new DoctorInfo()
            {
                Specialization = model.Specialization,
                DoctorId = doctor.Id,
                About = model.About,
                ContactNumber = model.ContactNumber,
                Email = model.Email,
                FullName = model.FullName
            };

            await context.DoctorsInfo.AddAsync(doctorInfo);
            await context.SaveChangesAsync();

            await userManager.AddToRoleAsync(user, "Doctor");

            return await context.Doctors.ContainsAsync(doctor);
        }

        public async Task Edit(DoctorDetailsModel model)
        {
            var doc = await context.DoctorsInfo.FirstOrDefaultAsync(x => x.DoctorId == model.Id);

            var wasNull = false;

            if (doc == null)
            {
                wasNull = true;
                doc = new DoctorInfo();
            }

            var doctor = await context.Doctors.FindAsync(model.Id);

            doctor.Specialization = model.Specialization;
            doc.Email = model.Email;
            doc.About = model.About;
            doc.ContactNumber = model.ContactNumber;
            doc.FullName = model.FullName;
            doc.Specialization = model.Specialization;
            doc.DoctorId = model.Id;

            if (wasNull)
            {
                await context.DoctorsInfo.AddAsync(doc);
            }

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

        public async Task<(string, List<BookingDisplayModel>)> GetDoctorAppointments(int doctorId)
        {
            var doctor = await context.Doctors.FindAsync(doctorId);

            if (doctor is null)
            {
                return ("", new List<BookingDisplayModel>());
            }

            var apps = await context.Bookings
                .Where(x => x.DoctorId == doctorId)
                .Select(x => new BookingDisplayModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                    Name = x.User.FullName,
                    ServiceName = x.Service.Name
                }).ToListAsync();

            var user = await context.Users.FindAsync(doctor.UserId);

            return (user.FullName, apps);
        }

        public async Task<bool> RemoveAppointment(int appointmetId)
        {
            var app = await context.Bookings.FindAsync(appointmetId);

            if (app is null)
            {
                return false;
            }

            context.Bookings.Remove(app);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var doctor = await context.Doctors.FindAsync(id);

            if (doctor is null) return false;

            var bookings = context.Bookings.Where(x => x.DoctorId == id);
            context.RemoveRange(bookings);

            var services = context.Services.Where(x => x.DoctorId == id);
            context.RemoveRange(services);

            context.Doctors.Remove(doctor);
            await context.SaveChangesAsync();

            return !await context.Doctors.ContainsAsync(doctor);
        }
    }
}
