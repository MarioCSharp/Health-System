using HealthSystemCommon.Infrastructure;
using HealthSystem.Booking.Data;
using HealthSystem.Booking.Services.AppointmentService;
using HealthSystem.Booking.Services.ServiceService;
using Microsoft.EntityFrameworkCore;
using HealthSystem.Booking.Infrastructure;

namespace HealthSystem.Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<BookingDbContext>(options => options.UseSqlServer(connectionString));

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(80);
                options.ListenAnyIP(5046);
            });

            builder.Services.AddTransient<IAppointmentService, AppointmentService>();
            builder.Services.AddTransient<IServiceService, ServiceService>();

            builder.Services.AddTokenAuthentication(builder.Configuration);

            builder.Services.AddHttpClient<IAppointmentService, AppointmentService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5025");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddHttpClient<IServiceService, ServiceService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5196");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            builder.Services.AddHttpClient<IServiceService, ServiceService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5025");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection().UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });

            app.UseAuthentication().UseAuthorization();


            app.MapControllers();

            app.Initialize();

            app.Run();
        }
    }
}
