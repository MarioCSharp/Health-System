using CommunityToolkit.Maui;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.DoctorService;
using HealthProject.Services.HealthIssueService;
using HealthProject.Services.HospitalService;
using HealthProject.Services.ServiceService;
using HealthProject.ViewModels;
using Microsoft.Extensions.Logging;
using HealthProject.Views;
using HealthProject.Services.ProblemService;
using HealthProject.Services.DocumentService;
using HealthProject.Services.MedicationService;
using HealthProject.Services.AppointmentService;
using HealthProject.Services.LogbookService;
using Syncfusion.Maui.Core.Hosting;
using HealthProject.Services.DiagnosisService;
using Plugin.LocalNotification;

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

            builder.UseLocalNotification();

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddHospitalPage>();
            builder.Services.AddTransient<AddDoctorPage>();
            builder.Services.AddTransient<DocumentAddPage>();
            builder.Services.AddTransient<DocumentViewPage>();
            builder.Services.AddTransient<DocumentDetailsPage>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<AddHospitalViewModel>();
            builder.Services.AddTransient<AddDoctorPageViewModel>();
            builder.Services.AddTransient<HealthIssueAddViewModel>();
            builder.Services.AddTransient<HealthIssuePageViewModel>();
            builder.Services.AddTransient<HealtIssueDetailsViewModel>();
            builder.Services.AddTransient<DocumentAddViewModel>();
            builder.Services.AddTransient<DocumentDetailsViewModel>();
            builder.Services.AddTransient<DocumetnsViewModel>();
            builder.Services.AddTransient<ProblemAddViewModel>();
            builder.Services.AddTransient<ProblemDetailsViewModel>();
            builder.Services.AddTransient<ProblemsViewPageViewModel>();
            builder.Services.AddTransient<MedicationAddViewModel>();
            builder.Services.AddTransient<MedicationDetailsViewModel>();
            builder.Services.AddTransient<MedicationPageViewModel>();
            builder.Services.AddTransient<AppointmentHistoryViewModel>();
            builder.Services.AddTransient<LogbookPageViewModel>();
            builder.Services.AddTransient<LogbookAddViewModel>();
            builder.Services.AddTransient<LogbookEditViewModel>();
            builder.Services.AddTransient<MedicationScheduleViewModel>();
            builder.Services.AddTransient<MyPrescriptionsViewModel>();
            builder.Services.AddTransient<DiagnosisViewModel>();
            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
            builder.Services.AddHttpClient<IHospitalService, HospitalService>();
            builder.Services.AddHttpClient<IServiceService, ServiceService>();
            builder.Services.AddHttpClient<IDoctorService, DoctorService>();
            builder.Services.AddHttpClient<IHealthIssueService, HealthIssueService>();
            builder.Services.AddHttpClient<IProblemService, ProblemService>();
            builder.Services.AddHttpClient<IDocumentService, DocumentService>();
            builder.Services.AddHttpClient<IMedicationService, MedicationService>();
            builder.Services.AddHttpClient<IAppointmentService, AppointmentService>();
            builder.Services.AddHttpClient<ILogbookService, LogbookService>();
            builder.Services.AddHttpClient<IDiagnosisService, DiagnosisService>();
            builder.Services.AddTransient<IDoctorService, DoctorService>();
            builder.Services.AddTransient<IHospitalService, HospitalService>();
            builder.Services.AddTransient<IServiceService, ServiceService>();
            builder.Services.AddTransient<IHealthIssueService, HealthIssueService>();
            builder.Services.AddTransient<IProblemService, ProblemService>();
            builder.Services.AddTransient<IDocumentService, DocumentService>();
            builder.Services.AddTransient<IMedicationService, MedicationService>();
            builder.Services.AddTransient<IAppointmentService, AppointmentService>();
            builder.Services.AddTransient<ILogbookService, LogbookService>();
            builder.Services.AddTransient<IDiagnosisService, DiagnosisService>();
            builder.Services.AddTransient<HospitalDetailsPage>();
            builder.Services.AddTransient<MyPrescriptionsPage>();
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
            builder.Services.AddTransient<MedicationDetailsPage>();
            builder.Services.AddTransient<MedicationAddPage>();
            builder.Services.AddTransient<MedicationViewPage>();
            builder.Services.AddTransient<AppointmentHistoryPage>();
            builder.Services.AddTransient<LogbookAddPage>();
            builder.Services.AddTransient<LogbookEditPage>();
            builder.Services.AddTransient<LogbookViewPage>();
            builder.Services.AddTransient<MedicationSchedulePage>();
            builder.Services.AddTransient<DiagnosisPredictionPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.ConfigureSyncfusionCore();
            return builder.Build();
        }
    }
}
