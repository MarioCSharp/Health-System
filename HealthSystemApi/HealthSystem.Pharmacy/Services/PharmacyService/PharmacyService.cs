using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Models.Pharmacy;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Pharmacy.Services.PharmacyService
{
    /// <summary>
    /// Service responsible for managing pharmacy-related operations.
    /// </summary>
    public class PharmacyService : IPharmacyService
    {
        private readonly PharmacyDbContext context;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PharmacyService"/> class.
        /// </summary>
        /// <param name="context">The <see cref="PharmacyDbContext"/> used for database operations.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> used for making HTTP requests.</param>
        public PharmacyService(PharmacyDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Adds a new pharmacy to the system.
        /// </summary>
        /// <param name="model">The <see cref="PharmacyAddModel"/> containing the pharmacy's details.</param>
        /// <param name="token">The authorization token for making external API requests.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> AddAsync(PharmacyAddModel model, string token)
        {
            var pharmacy = new Data.Models.Pharmacy()
            {
                Name = model.PharmacyName,
                Location = model.Location,
                ContactNumber = model.ContactNumber,
                OwnerUserId = model.OwnerUserId
            };

            // Assign the pharmacy owner role to the user
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://identity/api/Authentication/PutToRole?userId={model.OwnerUserId}&role=PharmacyOwner");

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

        /// <summary>
        /// Retrieves all pharmacies in the system.
        /// </summary>
        /// <returns>A list of <see cref="PharmacyModel"/> representing all pharmacies.</returns>
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

        /// <summary>
        /// Deletes a pharmacy from the system.
        /// </summary>
        /// <param name="id">The ID of the pharmacy to delete.</param>
        /// <param name="token">The authorization token for making external API requests.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> DeleteAsync(int id, string token)
        {
            var pharmacy = await context.Pharmacies.FindAsync(id);

            if (pharmacy is null)
            {
                return false;
            }

            // Remove the pharmacy owner role from the user
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
               $"http://identity/api/Authentication/DeleteFromRole?userId={pharmacy.OwnerUserId}&role=PharmacyOwner");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                // Delete related records
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Medications WHERE PharmacyId = {id}");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Pharmacists WHERE PharmacyId = {id}");
                await context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM UserCarts WHERE PharmacyId = {id}");

                context.Pharmacies.Remove(pharmacy);
                await context.SaveChangesAsync();

                return !await context.Pharmacies.ContainsAsync(pharmacy);
            }

            return false;
        }

        /// <summary>
        /// Retrieves detailed information about a specific pharmacy.
        /// </summary>
        /// <param name="id">The ID of the pharmacy to retrieve.</param>
        /// <returns>A <see cref="PharmacyModel"/> containing the pharmacy's details.</returns>
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

        /// <summary>
        /// Edits an existing pharmacy's information.
        /// </summary>
        /// <param name="model">The <see cref="PharmacyEditModel"/> containing updated pharmacy details.</param>
        /// <param name="userId">The ID of the user attempting to edit the pharmacy.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public async Task<bool> EditAsync(PharmacyEditModel model, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var pharmacy = await context.Pharmacies.FindAsync(model.Id);

            if (pharmacy is null)
            {
                return false;
            }

            // Check if the user has permission to edit the pharmacy
            if (pharmacy.OwnerUserId != userId && userId != "Administrator")
            {
                return false;
            }

            pharmacy.Location = model.Location;
            pharmacy.ContactNumber = model.ContactNumber;
            pharmacy.Name = model.Name;
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves pharmacy information based on the user's ID and role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>A <see cref="PharmacyModel"/> containing the pharmacy's details.</returns>
        public async Task<PharmacyModel> GetPharmacyByUserIdAsync(string userId, string role)
        {
            if (role == "PharmacyOwner")
            {
                var pharmacy = await context.Pharmacies.FirstOrDefaultAsync(p => p.OwnerUserId == userId);

                if (pharmacy is null)
                {
                    return new PharmacyModel();
                }

                return new PharmacyModel()
                {
                    Id = pharmacy.Id,
                    Name = pharmacy.Name,
                    Location = pharmacy.Location,
                    ContactNumber = pharmacy.ContactNumber
                };
            }
            else
            {
                var pharmacist = await context.Pharmacists.FirstOrDefaultAsync(p => p.UserId == userId);

                if (pharmacist is null)
                {
                    return new PharmacyModel();
                }

                var pharmacy = await context.Pharmacies.FindAsync(pharmacist.PharmacyId);

                if (pharmacy is null)
                {
                    return new PharmacyModel();
                }

                return new PharmacyModel()
                {
                    Id = pharmacy.Id,
                    Name = pharmacy.Name,
                    Location = pharmacy.Location,
                    ContactNumber = pharmacy.ContactNumber
                };
            }
        }
    }
}
