
using HealthSystem.Admins.Data;
using HealthSystem.Admins.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace HealthSystem.Admins.Services.RecepcionistService
{
    public class RecepcionistService : IRecepcionistService
    {
        private AdminsDbContext context;
        private HttpClient httpClient;

        public RecepcionistService(AdminsDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        public async Task<bool> AddAsync(string userId, int hospitalId, string name)
        {
            var recepcionist = new Recepcionist()
            {
                UserId = userId,
                Name = name,
                HospitalId = hospitalId
            };

            await httpClient.GetAsync($"http://localhost:5196/api/Authentication/PutToRole?userId={userId}&role={"Recepcionist"}");

            await context.Recepcionists.AddAsync(recepcionist);
            await context.SaveChangesAsync();

            return await context.Recepcionists.ContainsAsync(recepcionist);
        }

        public async Task<int> GetHospitalIdAsync(string userId)
        {
            var rec = await context.Recepcionists.FirstOrDefaultAsync(x => x.UserId == userId);

            if (rec is null) 
            {
                return -1;
            }

            return rec.HospitalId;
        }
    }
}
