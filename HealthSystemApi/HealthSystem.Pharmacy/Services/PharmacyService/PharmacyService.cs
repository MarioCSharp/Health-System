using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Models.Pharmacy;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace HealthSystem.Pharmacy.Services.PharmacyService
{
    public class PharmacyService : IPharmacyService
    {
        private PharmacyDbContext context;
        private HttpClient httpClient;

        public PharmacyService(PharmacyDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        public async Task<bool> AddAsync(PharmacyAddModel model, string token)
        {
            var pharmacy = new Data.Models.Pharmacy()
            {
                Name = model.PharmacyName,
                Location = model.Location,
                ContactNumber = model.ContactNumber,
                OwnerUserId = model.OwnerUserId
            };

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://localhost:5196/api/Authentication/PutToRole?userId={model.OwnerUserId}&role=PharmacyOwner");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                await context.Pharmacies.AddAsync(pharmacy);
                await context.SaveChangesAsync();

                return await context.Pharmacies.ContainsAsync(pharmacy);
            }

            return false;
        }

        public async Task<List<PharmacyModel>> AllAsync()
        {
            return await context.Pharmacies
                .Select(x => new PharmacyModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ContactNumber = x.ContactNumber,
                    Location = x.Location
                }).ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id, string token)
        {
            var pharmacy = await context.Pharmacies.FindAsync(id);

            if (pharmacy is null)
            {
                return false;
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
               $"http://localhost:5196/api/Authentication/DeleteFromRole?userId={pharmacy.OwnerUserId}&role=PharmacyOwner");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Medications WHERE PharmacyId = {id}");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Pharmacists WHERE PharmacyId = {id}");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM PharmacyOwners WHERE PharmacyId = {id}");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM UserCarts WHERE PharmacyId = {id}");

                context.Pharmacies.Remove(pharmacy);
                await context.SaveChangesAsync();

                return await context.Pharmacies.ContainsAsync(pharmacy);
            }

            return false;
        }

        public async Task<PharmacyModel> DetailsAsync(int id)
        {
            var pharmacy = await context.Pharmacies.FindAsync(id);

            if (pharmacy is null)
            {
                return new PharmacyModel();
            }

            return new PharmacyModel()
            {
                Id = id,
                ContactNumber = pharmacy.ContactNumber,
                Location = pharmacy.Location,
                Name = pharmacy.Name
            };
        }

        public async Task<bool> EditAsync(PharmacyEditModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var pharmacy = await context.Pharmacies.FindAsync(model.Id);

            if (pharmacy is null || pharmacy.OwnerUserId != userId)
            {
                return false;
            }

            pharmacy.Location = model.Location;
            pharmacy.ContactNumber = model.ContactNumber;
            pharmacy.Name = model.Name;
            await context.SaveChangesAsync();

            return true;
        }
    }
}
