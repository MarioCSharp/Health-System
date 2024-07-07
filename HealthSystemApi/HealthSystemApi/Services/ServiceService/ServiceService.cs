using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private ApplicationDbContext context;

        public ServiceService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(ServiceAddModel model)
        {
            var service = new Service()
            {
                Name = model.Name,
                Price = model.Price,
                Location = model.Location,
                DoctorId = model.DoctorId
            };

            await context.Services.AddAsync(service);
            await context.SaveChangesAsync();

            return await context.Services.ContainsAsync(service);
        }

        public async Task<List<ServiceModel>> AllByIdAsync(int id)
        {
            return await context.Services.Where(x => x.DoctorId == id)
                .Select(x => new ServiceModel()
                {
                    Name = x.Name,
                    Price = x.Price
                }).ToListAsync();
        }
    }
}
