using HealthSystem.Identity.Data;
using HealthSystem.Identity.Data.Models;
using HealthSystem.Identity.Services.IdentityService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using HealthSystemCommon.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using HealthSystem.Identity.Infrastructure;
using HealthSystem.Identity.Services.AccountService;

namespace HealthSystem.Identity
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
            builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddAuthorization();

            builder.Services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>().AddRoles<IdentityRole>();

            builder.Services.AddTransient<IIdentityService, IdentityService>();
            builder.Services.AddTransient<IAccountService, AccountService>();

            builder.Services.AddTokenAuthentication(builder.Configuration);

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
