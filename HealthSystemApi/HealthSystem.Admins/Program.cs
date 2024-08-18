using HealthSystem.Admins.Data;
using HealthSystem.Admins.Services.DoctorService;
using HealthSystem.Admins.Services.HospitalService;
using Microsoft.EntityFrameworkCore;
using HealthSystemCommon.Infrastructure;
using HealthSystem.Admins.Models;

namespace HealthSystem.Admins
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
            builder.Services.AddDbContext<AdminsDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddTransient<IHospitalService, HospitalService>();
            builder.Services.AddTransient<IDoctorService, DoctorService>();

            builder.Services.AddTokenAuthentication(builder.Configuration);
            
            builder.Services.AddHttpClient<IDoctorService, DoctorService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5196");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            });
            builder.Services.AddHttpClient<IDoctorService, DoctorService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5046");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            });
            builder.Services.AddHttpClient<IHospitalService, HospitalService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5196");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
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

            app.Run();
        }
    }
}
