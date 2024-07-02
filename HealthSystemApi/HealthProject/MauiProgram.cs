using HealthProject.Services.AuthenticationService;
using HealthProject.Services.DoctorService;
using HealthProject.Services.HospitalService;
using HealthProject.Services.NavigationService;
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
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddHospitalPage>();
            builder.Services.AddTransient<AddDoctorPage>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<AddHospitalViewModel>();
            builder.Services.AddTransient<AddDoctorPageViewModel>();
            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddHttpClient<IHospitalService, HospitalService>();
            builder.Services.AddHttpClient<IDoctorService, DoctorService>();
            builder.Services.AddTransient<INavigationService, NavigationService>();
            builder.Services.AddTransient<IDoctorService, DoctorService>();
            builder.Services.AddTransient<HospitalDetailsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
