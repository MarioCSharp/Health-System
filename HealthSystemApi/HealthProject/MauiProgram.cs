using HealthProject.Services;
using HealthProject.ViewModels;
using Microsoft.Extensions.Logging;

namespace HealthProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
