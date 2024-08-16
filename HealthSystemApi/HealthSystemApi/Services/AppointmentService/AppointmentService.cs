using HealthSystemApi.Data;
using HealthSystemApi.Models.Appointment;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace HealthSystemApi.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService   
    {
        private ApplicationDbContext context;

        public AppointmentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<AppointmentModel>> GetNextAppointmentsByDoctorUserId(string token)
        {
            var t = new JwtSecurityToken(token);

            var userId = t.Subject;

            var doctor = await context.Doctors.FirstOrDefaultAsync(x => x.UserId == userId);

            if (doctor == null) 
            {
                return new List<AppointmentModel>();
            }

            return await context.Bookings
                .Where(x => x.DoctorId == doctor.Id && x.Date > DateTime.Now)
                .OrderBy(x => x.Date)
                .Select(x => new AppointmentModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                    PatientName = x.User.FullName,
                    ServiceName = x.Service.Name
                }).ToListAsync();
        }

        public async Task<List<AppointmentModel>> GetPastAppointmentsByDoctorUserId(string token)
        {
            var t = new JwtSecurityToken(token);

            var userId = t.Subject;

            var doctor = await context.Doctors.FirstOrDefaultAsync(x => x.UserId == userId);

            if (doctor == null)
            {
                return new List<AppointmentModel>();
            }

            return await context.Bookings
                .Where(x => x.DoctorId == doctor.Id && x.Date < DateTime.Now)
                .OrderBy(x => x.Date)
                .Select(x => new AppointmentModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                    PatientName = x.User.FullName,
                    ServiceName = x.Service.Name
                }).ToListAsync();
        }

        public async Task<bool> Remove(int id)
        {
            var app = await context.Bookings.FindAsync(id);

            if (app is null)
            {
                return false;
            }

            context.Bookings.Remove(app);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
