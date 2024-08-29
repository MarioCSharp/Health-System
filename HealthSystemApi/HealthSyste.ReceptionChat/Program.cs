using HealthSyste.ReceptionChat.Services.RecepcionistService;
using HealthSystem.ReceptionChat.Hubs;
using HealthSystemCommon.Infrastructure;

namespace HealthSyste.ReceptionChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IRecepcionistService, RecepcionistService>();
            builder.Services.AddSingleton<ChatHub>();
            builder.Services.AddHttpClient();

            builder.Services.AddTokenAuthentication(builder.Configuration);

            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:5173") // Allow your frontend origin
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials(); // Allow credentials
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowSpecificOrigin");

            app.MapHub<ChatHub>("/chat");

            app.MapControllers();

            app.Run();
        }
    }
}
