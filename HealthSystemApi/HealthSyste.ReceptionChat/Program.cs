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

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseAuthorization().UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });

            app.MapHub<ChatHub>("/chat");

            app.MapControllers();

            app.Run();
        }
    }
}
