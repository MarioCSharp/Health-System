using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Pharmacist;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace HealthSystem.Pharmacy.Services.PharmacistService
{
    /// <summary>
    /// Service responsible for handling operations related to pharmacists in the pharmacy system.
    /// </summary>
    public class PharmacistService : IPharmacistService
    {
        private readonly PharmacyDbContext context;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PharmacistService"/> class.
        /// </summary>
        /// <param name="context">The <see cref="PharmacyDbContext"/> used for database operations.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> used for making HTTP requests.</param>
        public PharmacistService(PharmacyDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Adds a new pharmacist to a specific pharmacy.
        /// </summary>
        /// <param name="model">The <see cref="PharmacistAddModel"/> containing the pharmacist's details.</param>
        /// <param name="userId">The ID of the user adding the pharmacist.</param>
        /// <param name="token">The authorization token for making external API requests.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> AddAsync(PharmacistAddModel model, string userId, string token)
        {
            var pharmacy = await context.Pharmacies.FindAsync(model.PharmacyId);

            if (pharmacy is null)
            {
                return false;
            }

            // Check if the user is the owner or an administrator
            if (userId != pharmacy.OwnerUserId && userId != "Administrator")
            {
                return false;
            }

            // Assign the pharmacist role to the user
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://identity/api/Authentication/PutToRole?userId={model.UserId}&role=Pharmacist");

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

        /// <summary>
        /// Retrieves a list of pharmacists associated with a specific pharmacy.
        /// </summary>
        /// <param name="pharmacyId">The ID of the pharmacy to retrieve pharmacists for.</param>
        /// <returns>A list of <see cref="PharmacistDisplayModel"/> representing the pharmacists.</returns>
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

        /// <summary>
        /// Deletes a pharmacist from the system.
        /// </summary>
        /// <param name="pharmacistId">The ID of the pharmacist to delete.</param>
        /// <param name="token">The authorization token for making external API requests.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteAsync(int pharmacistId, string token)
        {
            var pharmacist = await context.Pharmacists.FindAsync(pharmacistId);

            if (pharmacist == null)
            {
                return false;
            }

            // Remove the pharmacist role from the user
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
               $"http://identity/api/Authentication/DeleteFromRole?userId={pharmacist.UserId}&role=Pharmacist");

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
