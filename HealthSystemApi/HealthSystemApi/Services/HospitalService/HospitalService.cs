using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Doctor;
using HealthSystemApi.Models.Hospital;
using HealthSystemApi.Services.AuthenticationService;
using HealthSystemApi.Services.DoctorService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.HospitalService
{
    public class HospitalService : IHospitalService
    {
        private ApplicationDbContext context;
        private UserManager<User> userManager;
        private IAuthenticationService authenticationService;
        private IDoctorService doctorService;

        public HospitalService(ApplicationDbContext context,
                               UserManager<User> userManager,
                               IDoctorService doctorService)
        {
            this.context = context;
            this.userManager = userManager;
            this.doctorService = doctorService;
        }

        public async Task<bool> AddAsync(HospitalAddModel model)
        {
            var user = await context.Users.FindAsync(model.OwnerId);

            if (user is null)
            {
                return false;
            }

            var hospital = new Hospital()
            {
                Name = model.HospitalName,
                ContactNumber = model.ContactNumber,
                Location = model.Location,
                OwnerId = model.OwnerId
            };

            await context.Hospitals.AddAsync(hospital);
            await context.SaveChangesAsync();

            await userManager.AddToRoleAsync(user, "Director");

            return await context.Hospitals.ContainsAsync(hospital);
        }

        public async Task<List<HospitalModel>> AllAsync()
        => await context.Hospitals.Select(x => new HospitalModel()
        {
            Id = x.Id,
            HospitalName = x.Name,
        }).ToListAsync();

        public async Task<bool> EditAsync(HospitalEditModel model)
        {
            var hospital = await context.Hospitals.FindAsync(model.Id);

            if (hospital == null)
            {
                return false;
            }

            hospital.Name = model.HospitalName;
            hospital.ContactNumber = model.HospitalContactNumber;
            hospital.Location = model.HospitalLocation;
            hospital.OwnerId = model.HospitalUserId;

            await context.SaveChangesAsync();

            return true;    
        }

        public async Task<List<DoctorDisplayModel>> GetDoctorsAsync(int id)
        {
            return await context.Doctors
                .Where(x => x.HospitalId == id)
                .Select(x => new DoctorDisplayModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    FullName = x.User.FullName,
                    Email = x.User.Email,
                    Specialization = x.Specialization
                }).ToListAsync();
        }

        public async Task<HospitalDetailsModel> GetHospital(int id)
        {
            var hospital = await context.Hospitals.FindAsync(id);

            if (hospital == null)
            {
                return new HospitalDetailsModel();
            }

            return new HospitalDetailsModel() 
            { 
                ContactNumber = hospital.ContactNumber,
                HospitalName = hospital.Name,
                Location = hospital.Location,
                UserId = hospital.OwnerId
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

        public async Task<bool> RemoveAsync(int id)
        {
            var hospital = await context.Hospitals.FindAsync(id);

            if (hospital == null)
            {
                return false;
            }

            var doctors = await context.Doctors.Where(x => x.HospitalId == id).ToListAsync();

            foreach (var doctor in doctors)
            {
                await doctorService.RemoveAsync(doctor.Id);
            }

            context.Hospitals.Remove(hospital);
            await context.SaveChangesAsync();

            return !await context.Hospitals.ContainsAsync(hospital);
        }
    }
}
