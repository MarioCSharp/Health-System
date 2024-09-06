using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Infrastructure;
using HealthSystem.Pharmacy.Services.CartService;
using HealthSystem.Pharmacy.Services.MedicationService;
using HealthSystem.Pharmacy.Services.OrderService;
using HealthSystem.Pharmacy.Services.PharmacistService;
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

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(80);
                options.ListenAnyIP(5171);
            });

            builder.Services.AddTokenAuthentication(builder.Configuration);

            builder.Services.AddHttpClient();

            builder.Services.AddTransient<IPharmacyService, PharmacyService>();
            builder.Services.AddTransient<IPharmacistService, PharmacistService>();
            builder.Services.AddTransient<IMedicationService, MedicationService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<ICartService, CartService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Initialize();

            app.Run();
        }
    }
}
