using HealthSystem.Admins.Data;
using HealthSystem.Admins.Data.Models;
using HealthSystem.Admins.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> AddAsync(string userId, int hospitalId, string name, string token)
        {
            var recepcionist = new Recepcionist()
            {
                UserId = userId,
                Name = name,
                HospitalId = hospitalId
            };

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://identity/api/Authentication/PutToRole?userId={recepcionist.UserId}&role=Recepcionist");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                await context.Recepcionists.AddAsync(recepcionist);
                await context.SaveChangesAsync();

                return await context.Recepcionists.ContainsAsync(recepcionist);
            }

            return false;
        }

        public async Task Delete(int id, string token)
        {
            var recepcionist = await context.Recepcionists.FindAsync(id);

            if (recepcionist is null)
            {
                return;
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://identity/api/Authentication/DeleteFromRole?userId={recepcionist.UserId}&role=Recepcionist");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                context.Recepcionists.Remove(recepcionist);
                await context.SaveChangesAsync();
            }
            else
            {
                return;
            }
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

        public async Task<List<RecepcionistDisplayModel>> GetMyRecepcionists(string userId)
        {
            return await context.Recepcionists
                .Where(x => x.Hospital.OwnerId == userId)
                .Select(x => new RecepcionistDisplayModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }
    }
}
