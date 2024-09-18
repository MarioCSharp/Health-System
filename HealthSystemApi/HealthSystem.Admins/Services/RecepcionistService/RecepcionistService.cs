using HealthSystem.Admins.Data;
using HealthSystem.Admins.Data.Models;
using HealthSystem.Admins.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Admins.Services.RecepcionistService
{
    public class RecepcionistService : IRecepcionistService
    {
        private AdminsDbContext context; // Database context for accessing recepcionist data
        private HttpClient httpClient; // HTTP client for making external API calls

        public RecepcionistService(AdminsDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Adds a new recepcionist to the database and assigns them a role.
        /// </summary>
        /// <param name="userId">ID of the user to be added as recepcionist.</param>
        /// <param name="hospitalId">ID of the hospital where the recepcionist will work.</param>
        /// <param name="name">Name of the recepcionist.</param>
        /// <param name="token">Authorization token.</param>
        /// <returns>True if the recepcionist was added successfully; otherwise, false.</returns>
        public async Task<bool> AddAsync(string userId, int hospitalId, string name, string token)
        {
            var recepcionist = new Recepcionist()
            {
                UserId = userId,
                Name = name,
                HospitalId = hospitalId
            };

            // Assign the user as a recepcionist in the identity service
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://identity/api/Authentication/PutToRole?userId={recepcionist.UserId}&role=Recepcionist");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                // Add the recepcionist to the database if the role assignment is successful
                await context.Recepcionists.AddAsync(recepcionist);
                await context.SaveChangesAsync();

                return await context.Recepcionists.ContainsAsync(recepcionist); // Check if the recepcionist was added successfully
            }

            return false; // Failed to add recepcionist
        }

        /// <summary>
        /// Deletes a recepcionist from the database and removes their role.
        /// </summary>
        /// <param name="id">ID of the recepcionist to be deleted.</param>
        /// <param name="token">Authorization token.</param>
        public async Task Delete(int id, string token)
        {
            var recepcionist = await context.Recepcionists.FindAsync(id);

            if (recepcionist is null)
            {
                return; // Recepcionist not found, exit the method
            }

            // Remove the recepcionist's role from the identity service
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://identity/api/Authentication/DeleteFromRole?userId={recepcionist.UserId}&role=Recepcionist");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                // Remove the recepcionist from the database if the role removal is successful
                context.Recepcionists.Remove(recepcionist);
                await context.SaveChangesAsync();
            }
            else
            {
                return; // Failed to remove the role, exit the method
            }
        }

        /// <summary>
        /// Gets the hospital ID associated with a specific recepcionist user ID.
        /// </summary>
        /// <param name="userId">User ID of the recepcionist.</param>
        /// <returns>Hospital ID if found; otherwise, -1.</returns>
        public async Task<int> GetHospitalIdAsync(string userId)
        {
            var rec = await context.Recepcionists.FirstOrDefaultAsync(x => x.UserId == userId);

            if (rec is null)
            {
                return -1; // Recepcionist not found
            }

            return rec.HospitalId; // Return the hospital ID
        }

        /// <summary>
        /// Gets a list of recepcionists associated with the user's hospital.
        /// </summary>
        /// <param name="userId">User ID of the hospital owner.</param>
        /// <returns>List of recepcionist display models.</returns>
        public async Task<List<RecepcionistDisplayModel>> GetMyRecepcionists(string userId)
        {
            return await context.Recepcionists
                .Where(x => x.Hospital.OwnerId == userId) // Filter recepcionists by hospital owner ID
                .Select(x => new RecepcionistDisplayModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync(); // Return the list of recepcionists
        }
    }
}
