using HealthSystem.Problems.Data;
using HealthSystem.Problems.Extensions;
using HealthSystem.Problems.Services.ProblemService;
using Microsoft.EntityFrameworkCore;
using HealthSystemCommon.Infrastructure;

namespace HealthSystem.Problems
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
            builder.Services.AddDbContext<ProblemsDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddTransient<IProblemService, ProblemService>();

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

            app.UseAuthentication().UseAuthorization().Initialize();


            app.MapControllers();

            app.Run();
        }
    }
}
