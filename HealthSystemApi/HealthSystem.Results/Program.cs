using HealthSystem.Results.Data;
using HealthSystem.Results.Services.LaboratoryResultService;
using HealthSystem.Results.Services.RecipeService;
using HealthSystemCommon.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Results
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
            builder.Services.AddDbContext<ResultsDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddTransient<ILaboratoryResultService, LaboratoryResultService>();
            builder.Services.AddTransient<IRecipeService, RecipeService>();

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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
