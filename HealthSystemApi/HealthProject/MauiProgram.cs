using CommunityToolkit.Maui;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.DoctorService;
using HealthProject.Services.HealthIssueService;
using HealthProject.Services.HospitalService;
using HealthProject.Services.NavigationService;
using HealthProject.Services.ServiceService;
using HealthProject.ViewModels;
using Microsoft.Extensions.Logging;
using HealthProject.Views;
using HealthProject.Services.ProblemService;
namespace HealthProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
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
            builder.Services.AddTransient<HealthIssueAddViewModel>();
            builder.Services.AddTransient<HealthIssuePageViewModel>();
            builder.Services.AddTransient<HealtIssueDetailsViewModel>();
            builder.Services.AddTransient<ProblemAddViewModel>();
            builder.Services.AddTransient<ProblemDetailsViewModel>();
            builder.Services.AddTransient<ProblemsViewPageViewModel>();
            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddHttpClient<IHospitalService, HospitalService>();
            builder.Services.AddHttpClient<IServiceService, ServiceService>();
            builder.Services.AddHttpClient<IDoctorService, DoctorService>();
            builder.Services.AddHttpClient<IHealthIssueService, HealthIssueService>();
            builder.Services.AddHttpClient<IProblemService, ProblemService>();
            builder.Services.AddTransient<INavigationService, NavigationService>();
            builder.Services.AddTransient<IDoctorService, DoctorService>();
            builder.Services.AddTransient<IHospitalService, HospitalService>();
            builder.Services.AddTransient<IServiceService, ServiceService>();
            builder.Services.AddTransient<IHealthIssueService, HealthIssueService>();
            builder.Services.AddTransient<IProblemService, ProblemService>();
            builder.Services.AddTransient<HospitalDetailsPage>();
            builder.Services.AddTransient<EditDoctorInfo>();
            builder.Services.AddTransient<DoctorDetailsPage>();
            builder.Services.AddTransient<AddServicePage>();
            builder.Services.AddTransient<ServiceDetailsPage>();
            builder.Services.AddTransient<BookingPage>();
            builder.Services.AddTransient<BookingViewModel>();
            builder.Services.AddTransient<HealthIssueAddPage>();
            builder.Services.AddTransient<HealthIssuesPage>();
            builder.Services.AddTransient<HealtIssueDetailsPage>();
            builder.Services.AddTransient<ProblemAddPage>();
            builder.Services.AddTransient<ProblemDetailsPage>();
            builder.Services.AddTransient<ProblemsViewPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
