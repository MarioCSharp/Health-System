using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Extensions;
using HealthSystemApi.Services.DoctorService;
using HealthSystemApi.Services.HospitalService;
using HealthSystemApi.Services.ServiceService;
using HealthSystemApi.Services.AuthenticationService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using HealthSystemApi.Services.HealthIssueService;
using HealthSystemApi.Services.ProblemService;

namespace HealthSystemApi
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
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddAuthentication();
            builder.Services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddRoles<IdentityRole>();

            builder.Services.AddTransient<IHospitalService, HospitalService>();
            builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<IDoctorService, DoctorService>();
            builder.Services.AddTransient<IServiceService, ServiceService>();
            builder.Services.AddTransient<IHealthIssueService, HealthIssueService>();
            builder.Services.AddTransient<IProblemService, ProblemService>();

            builder.Services.AddAuthorization();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger()
                    .UseSwaggerUI();
            }

            app.MapIdentityApi<User>();

            app.UseHttpsRedirection()
                .UseAuthorization()
                .Initialize();

            app.MapControllers();

            app.Run();
        }
    }
}
