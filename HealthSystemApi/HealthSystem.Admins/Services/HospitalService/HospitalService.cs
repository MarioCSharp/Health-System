using HealthSystem.Admins.Data;
using HealthSystem.Admins.Data.Models;
using HealthSystem.Admins.Models;
using HealthSystem.Admins.Services.DoctorService;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace HealthSystem.Admins.Services.HospitalService
{
    public class HospitalService : IHospitalService
    {
        private AdminsDbContext context; // Database context for accessing hospital data
        private IDoctorService doctorService; // Service for managing doctors
        private HttpClient httpClient; // HTTP client for making external API calls

        public HospitalService(AdminsDbContext context,
                               IDoctorService doctorService,
                               HttpClient httpClient)
        {
            this.context = context;
            this.doctorService = doctorService;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Adds a new hospital to the database.
        /// </summary>
        /// <param name="model">Hospital add model containing hospital details.</param>
        /// <param name="token">Authorization token.</param>
        /// <returns>True if the hospital was added successfully; otherwise, false.</returns>
        public async Task<bool> AddAsync(HospitalAddModel model, string token)
        {
            var hospital = new Hospital()
            {
                Name = model.HospitalName,
                ContactNumber = model.ContactNumber,
                Location = model.Location,
                OwnerId = model.OwnerId
            };

            // Add the hospital to the database
            await context.Hospitals.AddAsync(hospital);
            await context.SaveChangesAsync();

            // Assign the owner as the director in the identity service
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"http://identity/api/Authentication/PutToRole?userId={model.OwnerId}&role=Director");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await context.Hospitals.ContainsAsync(hospital); // Check if the hospital was added successfully
        }

        /// <summary>
        /// Gets a list of all hospitals.
        /// </summary>
        /// <returns>List of hospital models.</returns>
        public async Task<List<HospitalModel>> AllAsync()
        => await context.Hospitals.Select(x => new HospitalModel()
        {
            Id = x.Id,
            HospitalName = x.Name,
            Location = x.Location
        }).ToListAsync();

        /// <summary>
        /// Edits the details of an existing hospital.
        /// </summary>
        /// <param name="model">Hospital edit model with updated details.</param>
        /// <param name="userId">ID of the user making the edit.</param>
        /// <returns>True if the edit was successful; otherwise, false.</returns>
        public async Task<bool> EditAsync(HospitalEditModel model, string userId)
        {
            var hospital = await context.Hospitals.FindAsync(model.Id);
            if (hospital == null)
            {
                return false; // Hospital not found
            }

            // Validate if the user owns the hospital
            if (!string.IsNullOrEmpty(userId))
            {
                var userHospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
                if (userHospital is null || hospital.Id != userHospital.Id)
                {
                    return false; // User does not own the hospital
                }
            }

            // Update hospital details
            hospital.Name = model.HospitalName;
            hospital.ContactNumber = model.HospitalContactNumber;
            hospital.Location = model.HospitalLocation;

            if (!string.IsNullOrEmpty(model.HospitalUserId))
            {
                hospital.OwnerId = model.HospitalUserId; // Update owner ID if provided
            }

            await context.SaveChangesAsync(); // Save changes to the database
            return true;
        }

        /// <summary>
        /// Gets a list of doctors associated with a specific hospital.
        /// </summary>
        /// <param name="id">Hospital ID.</param>
        /// <param name="userId">User ID of the requester.</param>
        /// <returns>List of doctor display models.</returns>
        public async Task<List<DoctorDisplayModel>> GetDoctorsAsync(int id, string userId)
        {
            // Validate if the user owns the hospital
            if (!string.IsNullOrEmpty(userId))
            {
                var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
                if (hospital is null || id != hospital.Id)
                {
                    return new List<DoctorDisplayModel>(); // User does not own the hospital
                }
            }

            // Get doctors associated with the hospital
            return await context.Doctors
                .Where(x => x.HospitalId == id)
                .Select(x => new DoctorDisplayModel()
                {
                    Email = x.Email,
                    Specialization = x.Specialization,
                    FullName = x.FullName,
                    Id = x.Id,
                    UserId = x.UserId,
                })
                .ToListAsync();
        }

        /// <summary>
        /// Gets the details of a specific hospital.
        /// </summary>
        /// <param name="id">Hospital ID.</param>
        /// <param name="userId">User ID of the requester.</param>
        /// <returns>Hospital details model.</returns>
        public async Task<HospitalDetailsModel> GetHospital(int id, string userId)
        {
            var hospital = await context.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return new HospitalDetailsModel(); // Hospital not found
            }

            // Validate if the user owns the hospital
            if (!string.IsNullOrEmpty(userId))
            {
                var userHospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
                if (userHospital is null || hospital.Id != userHospital.Id)
                {
                    return new HospitalDetailsModel(); // User does not own the hospital
                }
            }

            // Return hospital details
            return new HospitalDetailsModel()
            {
                ContactNumber = hospital.ContactNumber,
                HospitalName = hospital.Name,
                Location = hospital.Location,
                UserId = hospital.OwnerId
            };
        }

        /// <summary>
        /// Gets the hospital details associated with a specific token.
        /// </summary>
        /// <param name="token">Authorization token.</param>
        /// <returns>Hospital details model.</returns>
        public async Task<HospitalDetailsModel> GetHospitalByToken(string token)
        {
            var t = new JwtSecurityToken(token); // Decode the token to get user ID
            var userId = t.Subject;

            var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
            if (hospital == null)
            {
                return new HospitalDetailsModel(); // No hospital found for user
            }

            return new HospitalDetailsModel()
            {
                Id = hospital.Id,
                HospitalName = hospital.Name // Return hospital ID and name
            };
        }

        /// <summary>
        /// Gets the details of a hospital by ID.
        /// </summary>
        /// <param name="id">Hospital ID.</param>
        /// <returns>Hospital details model.</returns>
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

        /// <summary>
        /// Removes a hospital and its associated doctors from the database.
        /// </summary>
        /// <param name="id">Hospital ID.</param>
        /// <param name="token">Authorization token.</param>
        /// <returns>True if the hospital was removed successfully; otherwise, false.</returns>
        public async Task<bool> RemoveAsync(int id, string token)
        {
            var hospital = await context.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return false; // Hospital not found
            }

            // Get and remove associated doctors
            var doctors = await context.Doctors.Where(x => x.HospitalId == id).ToListAsync();
            foreach (var doctor in doctors)
            {
                await doctorService.RemoveAsync(doctor.Id, hospital.OwnerId, token); // Remove each doctor
            }

            // Remove the hospital's director role from identity service
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            $"http://identity/api/Authentication/DeleteFromRole?userId={hospital.OwnerId}&role=Director");

            httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.SendAsync(httpRequestMessage);

            // Remove the hospital from the database
            if (response.IsSuccessStatusCode)
            {
                context.Hospitals.Remove(hospital);
                await context.SaveChangesAsync();
                return !await context.Hospitals.AnyAsync(x => x.Id == id); // Check if the hospital was successfully removed
            }

            return false; // Failed to remove the hospital's director role
        }
    }
}
