using HealthSystem.Admins.Data;
using HealthSystem.Admins.Data.Models;
using HealthSystem.Admins.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace HealthSystem.Admins.Services.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private AdminsDbContext context;
        private HttpClient httpClient;
        public DoctorService(AdminsDbContext context,
                             HttpClient httpClient)
        {
            this.context = context;
            this.httpClient = httpClient;
        }

        public async Task<bool> AddAsync(DoctorAddModel model, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

                if (hospital is null)
                {
                    return false;
                }

                if (model.HospitalId != hospital.Id)
                {
                    return false;
                }
            }

            var doctor = new Doctor()
            {
                FullName = model.FullName,
                Email = model.Email,
                Specialization = model.Specialization,
                HospitalId = model.HospitalId,
                UserId = model.UserId,
            };

            await context.Doctors.AddAsync(doctor);
            await context.SaveChangesAsync();

            var doctorInfo = new DoctorInfo()
            {
                Specialization = model.Specialization,
                DoctorId = doctor.Id,
                About = model.About,
                ContactNumber = model.ContactNumber,
                Email = model.Email,
                FullName = model.FullName
            };

            await context.DoctorsInfo.AddAsync(doctorInfo);
            await context.SaveChangesAsync();

            var response = await httpClient.GetAsync($"http://localhost:5196/api/Authentication/PutToRole?userId={model.UserId}&role={"Doctor"}");

            return await context.Doctors.ContainsAsync(doctor);
        }

        public async Task<bool> AddRating(float rating, string comment, int doctorId, int appointmentId, string userId)
        {
            var response = await httpClient.GetAsync($"http://localhost:5046/api/Appointment/GetAppointment?id={appointmentId}");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var appointment = await response.Content.ReadFromJsonAsync<AppointmentModel>();

            if (appointment is null || appointment.UserId != userId || appointment.DoctorId != doctorId)
            {
                return false;
            }

            var exists = await context.DoctorRatings.AnyAsync(x => x.DoctorId == doctorId && x.AppointmentId == appointmentId);

            if (exists)
            {
                return false;
            }

            var doctoRating = new DoctorRating()
            {
                Rating = rating,
                Comment = comment,
                DoctorId = doctorId,
                AppointmentId = appointmentId
            };

            try
            {
                await context.DoctorRatings.AddAsync(doctoRating);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public async Task<bool> AppointmentHasRating(int appointmentId)
        {
            return await context.DoctorRatings.AnyAsync(x => x.AppointmentId == appointmentId);
        }

        public async Task Edit(DoctorDetailsModel model, string userId)
        {
            var doctor = await context.Doctors.FindAsync(model.Id);

            if (doctor is null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

                if (hospital is null)
                {
                    return;
                }

                if (hospital.Id != doctor.HospitalId)
                {
                    return;
                }
            }

            var doc = await context.DoctorsInfo.FirstOrDefaultAsync(x => x.DoctorId == model.Id);

            var wasNull = false;

            if (doc == null)
            {
                wasNull = true;
                doc = new DoctorInfo();
            }

            doctor.Specialization = model.Specialization;
            doc.Email = model.Email;
            doc.About = model.About;
            doc.ContactNumber = model.ContactNumber;
            doc.FullName = model.FullName;
            doc.Specialization = model.Specialization;
            doc.DoctorId = model.Id;

            if (wasNull)
            {
                await context.DoctorsInfo.AddAsync(doc);
            }

            await context.SaveChangesAsync();
        }

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

        public async Task<List<DoctorModel>> GetAllDoctorsByUserId(string userId)
        {
            var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

            if (hospital == null)
            {
                return new List<DoctorModel>();
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

        public async Task<DoctorDetailsModel> GetDetailsAsync(int id)
        {
            var info = await context.DoctorsInfo.FindAsync(id);

            return new DoctorDetailsModel
            {
                Id = info.Id,
                FullName = info.FullName,
                Specialization = info.Specialization,
                ContactNumber = info.ContactNumber,
                Email = info.Email,
                About = info.About
            };
        }

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

        public async Task<List<DoctorRatingDisplayModel>> GetDoctorRatings(int doctorId)
        => await context.DoctorRatings
                .Where(x => x.DoctorId == doctorId)
                .Select(x => new DoctorRatingDisplayModel()
                {
                    Id = x.Id,
                    Rating = x.Rating,
                    Comment = x.Comment
                }).ToListAsync();

        public async Task<List<DoctorModel>> GetTopDoctorsWithSpecialization(string specialization, int top)
        {
            return await context.Doctors
                    .Where(x => x.Specialization == specialization)
                    .OrderBy(x => context.DoctorRatings.Where(y => y.DoctorId == x.Id).Sum(x => x.Rating) / context.DoctorRatings.Where(y => y.DoctorId == x.Id).Count())
                    .Select(x => new DoctorModel()
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Specialization = x.Specialization
                    })
                    .ToListAsync();
        }

        public async Task<int> HospitalIdByDirector(string userId)
        {
            var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

            if (hospital == null)
            {
                return 0;
            }

            return hospital.Id;
        }

        public async Task<bool> RemoveAsync(int id, string userId, string token)
        {
            var doctor = await context.Doctors.FindAsync(id);

            if (doctor is null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var hospital = await context.Hospitals.FirstOrDefaultAsync(x => x.OwnerId == userId);

                if (hospital is null)
                {
                    return false;
                }

                if (doctor.HospitalId != hospital.Id)
                {
                    return false;
                }
            }

            var request1 = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5046/api/Appointment/DeleteAllByDoctorId?doctorId={id}");
            request1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.SendAsync(request1);
            
            var request2 = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5046/api/Service/DeleteAllByDoctorId?doctorId={id}");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response2 = await httpClient.SendAsync(request2);
            
            var request3 = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5196/api/Authentication/DeleteFromRole?userId={doctor.UserId}&role=Doctor");
            request3.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response3 = await httpClient.SendAsync(request3);

            context.Doctors.Remove(doctor);
            await context.SaveChangesAsync();

            return !await context.Doctors.ContainsAsync(doctor);
        }
    }
}
