using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Services.MedicationService;
using HealthSystem.Pharmacy.Services.OrderService;
using HealthSystem.Pharmacy.Services.PharmacyService;
using HealthSystemCommon.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Pharmacy
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
            builder.Services.AddDbContext<PharmacyDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddTokenAuthentication(builder.Configuration);

            builder.Services.AddTransient<IPharmacyService, PharmacyService>();
            builder.Services.AddTransient<IMedicationService, MedicationService>();
            builder.Services.AddTransient<IOrderService, OrderService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
