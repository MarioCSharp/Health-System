using HealthSystem.HealthCare.Data;
using HealthSystem.HealthCare.Services.HealthIssueService;
using HealthSystem.HealthCare.Services.LogbookService;
using HealthSystem.HealthCare.Services.MedicationService;
using Microsoft.EntityFrameworkCore;

using HealthSystemCommon.Infrastructure;

namespace HealthSystem.HealthCare
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
            builder.Services.AddDbContext<HealthCareDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddTransient<IHealthIssueService, HealthIssueService>();
            builder.Services.AddTransient<IMedicationService, MedicationService>();
            builder.Services.AddTransient<ILogbookService, LogbookService>();

            builder.Services.AddTokenAuthentication(builder.Configuration);

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
