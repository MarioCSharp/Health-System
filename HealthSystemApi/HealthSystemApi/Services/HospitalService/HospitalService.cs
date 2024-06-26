using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Hospital;
using HealthSystemApi.Services.AuthenticationService;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.HospitalService
{
    public class HospitalService : IHospitalService
    {
        private ApplicationDbContext context;
        private IAuthenticationService authenticationService;

        public HospitalService(ApplicationDbContext context)
        {
            this.context = context;
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

            return await context.Hospitals.ContainsAsync(hospital);
        }

        public async Task<List<HospitalModel>> AllAsync()
        =>  await context.Hospitals.Select(x => new HospitalModel()
            {
                Id = x.Id,
                HospitalName = x.Name,
            }).ToListAsync();

        public async Task<bool> RemoveAsync(int id)
        {
            var hospital = await context.Hospitals.FindAsync(id);

            if (hospital == null)
            {
                return false;
            }

            context.Hospitals.Remove(hospital);
            await context.SaveChangesAsync();

            return !await context.Hospitals.ContainsAsync(hospital);
        }
    }
}
