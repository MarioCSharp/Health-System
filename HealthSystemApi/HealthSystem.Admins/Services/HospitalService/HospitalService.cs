using HealthSystem.Admins.Data;
using HealthSystem.Admins.Data.Models;
using HealthSystem.Admins.Models;
using HealthSystem.Admins.Services.DoctorService;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace HealthSystem.Admins.Services.HospitalService
{
    public class HospitalService : IHospitalService
    {
        private AdminsDbContext context;
        private IDoctorService doctorService;
        private HttpClient httpClient;

        public HospitalService(AdminsDbContext context,
                               IDoctorService doctorService,
                               HttpClient httpClient)
        {
            this.context = context;
            this.doctorService = doctorService;
            this.httpClient = httpClient;
        }

        public async Task<bool> AddAsync(HospitalAddModel model)
        {
            var hospital = new Hospital()
            {
                Name = model.HospitalName,
                ContactNumber = model.ContactNumber,
                Location = model.Location,
                OwnerId = model.OwnerId
            };

            await context.Hospitals.AddAsync(hospital);
            await context.SaveChangesAsync();

            var response = await httpClient.GetAsync($"http://localhost:5196/api/Authentication/PutToRole?userId={model.OwnerId}&role={"Director"}");

            return await context.Hospitals.ContainsAsync(hospital);
        }

        public async Task<List<HospitalModel>> AllAsync()
        => await context.Hospitals.Select(x => new HospitalModel()
        {
            Id = x.Id,
            HospitalName = x.Name,
        }).ToListAsync();

        public async Task<bool> EditAsync(HospitalEditModel model, string userId)
        {
            var hospital = await context.Hospitals.FindAsync(model.Id);

            if (hospital == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var userHospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

                if (userHospital is null)
                {
                    return false;
                }

                if (hospital.Id != userHospital.Id)
                {
                    return false;
                }
            }

            hospital.Name = model.HospitalName;
            hospital.ContactNumber = model.HospitalContactNumber;
            hospital.Location = model.HospitalLocation;

            if (!string.IsNullOrEmpty(model.HospitalUserId))
            {
                hospital.OwnerId = model.HospitalUserId;
            }

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<DoctorDisplayModel>> GetDoctorsAsync(int id, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

                if (hospital is null)
                {
                    return new List<DoctorDisplayModel>();
                }

                if (id != hospital.Id)
                {
                    return new List<DoctorDisplayModel>();
                }
            }

            return await context.Doctors
                .Where(x => x.HospitalId == id)
                .Select(x => new DoctorDisplayModel()
                {
                    Email = x.Email,
                    Specialization = x.Specialization,
                    FullName = x.FullName,
                    Id = x.Id,
                    UserId = x.UserId,
                })
                .ToListAsync();
        }

        public async Task<HospitalDetailsModel> GetHospital(int id, string userId)
        {
            var hospital = await context.Hospitals.FindAsync(id);

            if (hospital == null)
            {
                return new HospitalDetailsModel();
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var userHospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

                if (userHospital is null)
                {
                    return new HospitalDetailsModel();
                }

                if (hospital.Id != userHospital.Id)
                {
                    return new HospitalDetailsModel();
                }
            }

            return new HospitalDetailsModel()
            {
                ContactNumber = hospital.ContactNumber,
                HospitalName = hospital.Name,
                Location = hospital.Location,
                UserId = hospital.OwnerId
            };
        }

        public async Task<HospitalDetailsModel> GetHospitalByToken(string token)
        {
            var t = new JwtSecurityToken(token);

            var userId = t.Subject;

            var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

            if (hospital == null)
            {
                return new HospitalDetailsModel();
            }

            return new HospitalDetailsModel()
            {
                Id = hospital.Id,
                HospitalName = hospital.Name
            };
        }

        public async Task<HospitalDetailsModel> HospitalDetails(int id)
        {
            var hospital = await context.Hospitals.FindAsync(id);

            return new HospitalDetailsModel
            {
                Id = id,
                HospitalName = hospital.Name,
                ContactNumber = hospital.ContactNumber,
                Location = hospital.Location,
            };
        }

        public async Task<bool> RemoveAsync(int id) // TODO: Optimize
        {
            var hospital = await context.Hospitals.FindAsync(id);

            if (hospital == null)
            {
                return false;
            }

            var doctors = await context.Doctors.Where(x => x.HospitalId == id).ToListAsync();

            foreach (var doctor in doctors)
            {
                await doctorService.RemoveAsync(doctor.Id, hospital.OwnerId);
            }

            context.Hospitals.Remove(hospital);
            await context.SaveChangesAsync();

            return !await context.Hospitals.ContainsAsync(hospital);
        }
    }
}
