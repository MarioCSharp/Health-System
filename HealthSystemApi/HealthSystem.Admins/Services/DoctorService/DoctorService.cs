using HealthSystem.Admins.Data;
using HealthSystem.Admins.Data.Models;
using HealthSystem.Admins.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace HealthSystem.Admins.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private AdminsDbContext context; // Database context for accessing data
        private HttpClient httpClient; // HTTP client for making external API calls

        public DoctorService(AdminsDbContext context, HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Adds a new doctor to the database.
        /// </summary>
        /// <param name="model">Doctor add model containing doctor information.</param>
        /// <param name="userId">ID of the user adding the doctor.</param>
        /// <param name="token">Authorization token.</param>
        /// <returns>True if the doctor was added successfully; otherwise, false.</returns>
        public async Task<bool> AddAsync(DoctorAddModel model, string userId, string token)
        {
            // Check if user has a hospital
            if (!string.IsNullOrEmpty(userId))
            {
                var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
                if (hospital is null)
                {
                    return false; // User doesn't own a hospital
                }

                // Check if the hospital ID matches
                if (model.HospitalId != hospital.Id)
                {
                    return false; // Hospital ID mismatch
                }
            }

            // Create a new Doctor object
            var doctor = new Doctor()
            {
                FullName = model.FullName,
                Email = model.Email,
                Specialization = model.Specialization,
                HospitalId = model.HospitalId,
                UserId = model.UserId,
            };

            // Add doctor to the database
            await context.Doctors.AddAsync(doctor);
            await context.SaveChangesAsync();

            // Create DoctorInfo object
            var doctorInfo = new DoctorInfo()
            {
                Specialization = model.Specialization,
                DoctorId = doctor.Id,
                About = model.About,
                ContactNumber = model.ContactNumber,
                Email = model.Email,
                FullName = model.FullName
            };

            // Send a request to assign the role to the user
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"http://identity/api/Authentication/PutToRole?userId={model.UserId}&role=Doctor");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                return false; // Role assignment failed
            }

            // Add doctor info to the database
            await context.DoctorsInfo.AddAsync(doctorInfo);
            await context.SaveChangesAsync();

            return await context.Doctors.ContainsAsync(doctor); // Check if the doctor was added successfully
        }

        /// <summary>
        /// Adds a rating for a specific doctor.
        /// </summary>
        /// <param name="rating">Rating value.</param>
        /// <param name="comment">Comment for the rating.</param>
        /// <param name="doctorId">ID of the doctor being rated.</param>
        /// <param name="appointmentId">ID of the appointment.</param>
        /// <param name="userId">ID of the user rating the doctor.</param>
        /// <returns>True if the rating was added successfully; otherwise, false.</returns>
        public async Task<bool> AddRating(float rating, string comment, int doctorId, int appointmentId, string userId)
        {
            // Check if the appointment exists
            var response = await httpClient.GetAsync($"http://booking/api/Appointment/GetAppointment?id={appointmentId}");
            if (!response.IsSuccessStatusCode)
            {
                return false; // Appointment not found
            }

            var appointment = await response.Content.ReadFromJsonAsync<AppointmentModel>();
            if (appointment is null || appointment.UserId != userId || appointment.DoctorId != doctorId)
            {
                return false; // Appointment user ID mismatch or invalid doctor ID
            }

            // Check if a rating already exists for this appointment and doctor
            var exists = await context.DoctorRatings.AnyAsync(x => x.DoctorId == doctorId && x.AppointmentId == appointmentId);
            if (exists)
            {
                return false; // Rating already exists
            }

            // Create a new rating object
            var doctoRating = new DoctorRating()
            {
                Rating = rating,
                Comment = comment,
                DoctorId = doctorId,
                AppointmentId = appointmentId
            };

            // Add the rating to the database
            try
            {
                await context.DoctorRatings.AddAsync(doctoRating);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log any exceptions
            }

            return true; // Rating added successfully
        }

        /// <summary>
        /// Checks if an appointment has a rating.
        /// </summary>
        /// <param name="appointmentId">ID of the appointment.</param>
        /// <returns>True if the appointment has a rating; otherwise, false.</returns>
        public async Task<bool> AppointmentHasRating(int appointmentId)
        {
            return await context.DoctorRatings.AnyAsync(x => x.AppointmentId == appointmentId);
        }

        /// <summary>
        /// Edits an existing doctor's details.
        /// </summary>
        /// <param name="model">Doctor details model containing updated information.</param>
        /// <param name="userId">ID of the user making the edit.</param>
        public async Task Edit(DoctorDetailsModel model, string userId)
        {
            var doctor = await context.Doctors.FindAsync(model.Id);
            if (doctor is null)
            {
                return; // Doctor not found
            }

            // Check if user has a hospital
            if (!string.IsNullOrEmpty(userId))
            {
                var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
                if (hospital is null)
                {
                    return; // User doesn't own a hospital
                }

                if (hospital.Id != doctor.HospitalId)
                {
                    return; // Hospital ID mismatch
                }
            }

            // Get or create the doctor info
            var doc = await context.DoctorsInfo.FirstOrDefaultAsync(x => x.DoctorId == model.Id);
            var wasNull = false;

            if (doc == null)
            {
                wasNull = true; // New DoctorInfo object needs to be created
                doc = new DoctorInfo();
            }

            // Update doctor and doctor info fields
            doctor.Specialization = model.Specialization;
            doc.Email = model.Email;
            doc.About = model.About;
            doc.ContactNumber = model.ContactNumber;
            doc.FullName = model.FullName;
            doc.Specialization = model.Specialization;
            doc.DoctorId = model.Id;

            if (wasNull)
            {
                await context.DoctorsInfo.AddAsync(doc); // Add new doctor info
            }

            await context.SaveChangesAsync(); // Save changes to the database
        }

        /// <summary>
        /// Gets all doctors for a specific hospital.
        /// </summary>
        /// <param name="id">Hospital ID.</param>
        /// <returns>List of doctors in the specified hospital.</returns>
        public async Task<List<DoctorModel>> GetAllAsync(int id)
        {
            return await context.Doctors.Where(x => x.HospitalId == id)
                .Select(x => new DoctorModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Specialization = x.Specialization
                }).ToListAsync();
        }

        /// <summary>
        /// Gets all doctors associated with a specific user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>List of doctors associated with the user.</returns>
        public async Task<List<DoctorModel>> GetAllDoctorsByUserId(string userId)
        {
            var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
            if (hospital == null)
            {
                return new List<DoctorModel>(); // No hospital found for user
            }

            return await context.Doctors
                .Where(x => x.HospitalId == hospital.Id)
                .Select(x => new DoctorModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Specialization = x.Specialization
                }).ToListAsync();
        }

        /// <summary>
        /// Gets the details of a specific doctor.
        /// </summary>
        /// <param name="id">Doctor ID.</param>
        /// <returns>Doctor details model.</returns>
        public async Task<DoctorDetailsModel> GetDetailsAsync(int id)
        {
            var info = await context.DoctorsInfo.FirstOrDefaultAsync(x => x.DoctorId == id);
            if (info is null)
            {
                return new DoctorDetailsModel(); // No information found for doctor
            }

            return new DoctorDetailsModel
            {
                Id = info.DoctorId,
                FullName = info.FullName,
                Specialization = info.Specialization,
                ContactNumber = info.ContactNumber,
                Email = info.Email,
                About = info.About
            };
        }

        /// <summary>
        /// Gets a doctor based on the ID.
        /// </summary>
        /// <param name="id">Doctor ID.</param>
        /// <returns>Doctor add model with associated details.</returns>
        public async Task<DoctorAddModel> GetDoctor(int id)
        {
            var info = await context.Doctors.FindAsync(id);
            return new DoctorAddModel()
            {
                HospitalId = info.HospitalId,
                Specialization = info.Specialization,
                UserId = info.UserId
            };
        }

        /// <summary>
        /// Gets a doctor based on the user ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>Doctor model.</returns>
        public async Task<DoctorModel> GetDoctorByUserId(string userId)
        {
            var doctor = await context.Doctors.FirstOrDefaultAsync(x => x.UserId == userId);
            return new DoctorModel()
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                Specialization = doctor.Specialization
            };
        }

        /// <summary>
        /// Gets ratings for a specific doctor.
        /// </summary>
        /// <param name="doctorId">ID of the doctor.</param>
        /// <returns>List of doctor rating display models.</returns>
        public async Task<List<DoctorRatingDisplayModel>> GetDoctorRatings(int doctorId)
        {
            return await context.DoctorRatings
                .Where(x => x.DoctorId == doctorId)
                .Select(x => new DoctorRatingDisplayModel()
                {
                    Id = x.Id,
                    Rating = x.Rating,
                    Comment = x.Comment
                }).ToListAsync();
        }

        /// <summary>
        /// Gets the top doctors with a specific specialization.
        /// </summary>
        /// <param name="specialization">Specialization to filter doctors.</param>
        /// <param name="top">Number of top doctors to return.</param>
        /// <returns>List of top doctors.</returns>
        public async Task<List<DoctorModel>> GetTopDoctorsWithSpecialization(string specialization, int top)
        {
            return await context.Doctors
                    .Where(x => x.Specialization.ToLower() == specialization.ToLower())
                    .Select(x => new
                    {
                        Doctor = x,
                        AverageRating = context.DoctorRatings
                            .Where(y => y.DoctorId == x.Id)
                            .Any() ? context.DoctorRatings
                                         .Where(y => y.DoctorId == x.Id)
                                         .Average(y => y.Rating) : 0
                    })
                    .OrderByDescending(x => x.AverageRating)
                    .Select(x => new DoctorModel()
                    {
                        Id = x.Doctor.Id,
                        FullName = x.Doctor.FullName,
                        Specialization = x.Doctor.Specialization
                    })
                    .ToListAsync();
        }

        /// <summary>
        /// Gets the hospital ID by the director's user ID.
        /// </summary>
        /// <param name="userId">User ID of the director.</param>
        /// <returns>Hospital ID associated with the director; returns 0 if not found.</returns>
        public async Task<int> HospitalIdByDirector(string userId)
        {
            var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
            if (hospital == null)
            {
                return 0; // No hospital found for user
            }

            return hospital.Id; // Return hospital ID
        }

        /// <summary>
        /// Removes a doctor from the database.
        /// </summary>
        /// <param name="id">Doctor ID.</param>
        /// <param name="userId">User ID of the person removing the doctor.</param>
        /// <param name="token">Authorization token.</param>
        /// <returns>True if the doctor was removed successfully; otherwise, false.</returns>
        public async Task<bool> RemoveAsync(int id, string userId, string token)
        {
            var doctor = await context.Doctors.FindAsync(id);
            if (doctor is null)
            {
                return false; // Doctor not found
            }

            // Check if user has a hospital
            if (!string.IsNullOrEmpty(userId))
            {
                var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);
                if (hospital is null)
                {
                    return false; // User doesn't own a hospital
                }

                if (doctor.HospitalId != hospital.Id)
                {
                    return false; // Hospital ID mismatch
                }
            }

            // Send requests to delete associated data
            var request1 = new HttpRequestMessage(HttpMethod.Get, $"http://booking/api/Appointment/DeleteAllByDoctorId?doctorId={id}");
            request1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.SendAsync(request1);

            var request2 = new HttpRequestMessage(HttpMethod.Get, $"http://booking/api/Service/DeleteAllByDoctorId?doctorId={id}");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = await httpClient.SendAsync(request2);

            var request3 = new HttpRequestMessage(HttpMethod.Get, $"http://identity/api/Authentication/DeleteFromRole?userId={doctor.UserId}&role=Doctor");
            request3.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response3 = await httpClient.SendAsync(request3);

            // Remove the doctor from the database
            context.Doctors.Remove(doctor);
            await context.SaveChangesAsync();

            return !await context.Doctors.ContainsAsync(doctor); // Check if the doctor was successfully removed
        }
    }
}
