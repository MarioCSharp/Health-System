using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private ApplicationDbContext context;

        public ServiceService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(ServiceAddModel model)
        {
            var service = new Service()
            {
                Name = model.Name,
                Price = model.Price,
                Location = model.Location,
                Description = model.Description,
                DoctorId = model.DoctorId
            };

            await context.Services.AddAsync(service);
            await context.SaveChangesAsync();

            return await context.Services.ContainsAsync(service);
        }

        public async Task<List<ServiceModel>> AllByIdAsync(int id)
        {
            return await context.Services.Where(x => x.DoctorId == id)
                .Select(x => new ServiceModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                }).ToListAsync();
        }

        public async Task<List<string>> AvailableHoursAsync(DateTime date, int serviceId)
        {
            var service = await context.Services.FindAsync(serviceId);

            var doctorId = service.DoctorId;

            var targetDate = date.Date;

            var doctorBookings = await context.Bookings
                .Where(x => x.DoctorId == doctorId && x.Date.Date == targetDate)
                .ToListAsync();

            var allHours = new List<string>()
            {
                "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00",
                "14:30", "15:00", "15:30", "16:00", "16:30"
            };

            foreach (var booking in doctorBookings)
            {
                var timeToString = booking.Date.TimeOfDay.ToString("hh\\:mm");

                allHours.Remove(timeToString);
            }

            return allHours;
        }

        public async Task<bool> BookAsync(BookingModel model)
        {
            var bookings = await context.Bookings
                .Where(x => x.Date.Day == model.Day &&
                            x.Date.Month == model.Month &&
                            x.Date.Year == model.Year &&
                            x.DoctorId == model.DoctorId).ToListAsync();

            var hour = 0;
            var minute = 0;

            if (model.Time[0] == '0')
            {
                hour = int.Parse(model.Time[1].ToString());
            }
            else
            {
                hour = int.Parse(model.Time[0].ToString() + model.Time[1].ToString());
            }

            if (model.Time[3] == '0')
            {
                minute = int.Parse(model.Time[4].ToString());
            }
            else
            {
                minute = int.Parse(model.Time[3].ToString() + model.Time[4].ToString());
            }

            foreach (var item in bookings) 
            {
                if (item.Date.Hour == hour && item.Date.Minute == minute)
                {
                    return false;
                }
            }

            var dateTime = new DateTime(model.Year, model.Month, model.Day, hour, minute, 0);

            var booking = new Booking()
            {
                Date = dateTime,
                UserId = model.UserId,
                ServiceId = model.ServiceId,
                DoctorId = model.DoctorId
            };

            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ServiceDetailsModel> DetailsAsync(int id)
        {
            var service = await context.Services.FindAsync(id);

            return new ServiceDetailsModel()
            {
                Id = id,
                DoctorId = service.DoctorId,
                Location = service.Location,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price
            };
        }
    }
}
