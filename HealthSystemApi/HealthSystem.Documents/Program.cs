using HealthSystemCommon.Infrastructure;
using HealthSystem.Documents.Data;
using HealthSystem.Documents.Services.DocumentService;
using Microsoft.EntityFrameworkCore;
using HealthSystem.Documents.Services.ReminderService;

namespace HealthSystem.Documents
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
            builder.Services.AddDbContext<DocumentsDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddTransient<IDocumentService, DocumentService>();
            builder.Services.AddTransient<IReminderService, ReminderService>();

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
