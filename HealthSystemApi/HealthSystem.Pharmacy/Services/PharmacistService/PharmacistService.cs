using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Pharmacist;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace HealthSystem.Pharmacy.Services.PharmacistService
{
    public class PharmacistService : IPharmacistService
    {
        private PharmacyDbContext context;
        private HttpClient httpClient;

        public PharmacistService(PharmacyDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        public async Task<bool> AddAsync(PharmacistAddModel model, string userId, string token)
        {
            var pharmacy = await context.Pharmacies.FindAsync(model.PharmacyId);

            if (pharmacy is null)
            {
                return false;
            }

            if (userId != pharmacy.OwnerUserId && userId != "Administrator")
            {
                return false;
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://localhost:5196/api/Authentication/PutToRole?userId={model.UserId}&role=Pharmacist");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                var pharmacist = new Pharmacist()
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserId = model.UserId,
                    PharmacyId = pharmacy.Id
                };

                await context.Pharmacists.AddAsync(pharmacist);
                await context.SaveChangesAsync();

                return await context.Pharmacists.ContainsAsync(pharmacist);
            }

            return false;
        }

        public async Task<List<PharmacistDisplayModel>> AllByPharmacyId(int pharmacyId)
        {
            return await context.Pharmacists
                .Where(x => x.PharmacyId == pharmacyId)
                .Select(x => new PharmacistDisplayModel()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<bool> DeleteAsync(int pharmacistId, string token)
        {
            var pharmacist = await context.Pharmacists.FindAsync(pharmacistId);

            if (pharmacist == null)
            {
                return false;
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
               $"http://localhost:5196/api/Authentication/DeleteFromRole?userId={pharmacist.UserId}&role=Pharmacist");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                context.Pharmacists.Remove(pharmacist);
                await context.SaveChangesAsync();

                return !await context.Pharmacists.ContainsAsync(pharmacist);
            }

            return false;
        }
    }
}
