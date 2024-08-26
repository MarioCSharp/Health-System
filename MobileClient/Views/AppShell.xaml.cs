using HealthProject.Services.AuthenticationService;

namespace HealthProject.Views
{
    public partial class AppShell : Shell
    {

        public AppShell(IAuthenticationService authenticationService)
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(AddHospitalPage), typeof(AddHospitalPage));
            Routing.RegisterRoute(nameof(HospitalDetailsPage), typeof(HospitalDetailsPage));
            Routing.RegisterRoute(nameof(AddDoctorPage), typeof(AddDoctorPage));
            Routing.RegisterRoute(nameof(DoctorDetailsPage), typeof(DoctorDetailsPage));
            Routing.RegisterRoute(nameof(EditDoctorInfo), typeof(EditDoctorInfo));
            Routing.RegisterRoute(nameof(AddServicePage), typeof(AddServicePage));
            Routing.RegisterRoute(nameof(ServiceDetailsPage), typeof(ServiceDetailsPage));
            Routing.RegisterRoute(nameof(BookingPage), typeof(BookingPage));
            Routing.RegisterRoute(nameof(HealthIssueAddPage), typeof(HealthIssueAddPage));
            Routing.RegisterRoute(nameof(HealthIssuesPage), typeof(HealthIssuesPage));
            Routing.RegisterRoute(nameof(HealtIssueDetailsPage), typeof(HealtIssueDetailsPage));
            Routing.RegisterRoute(nameof(ProblemAddPage), typeof(ProblemAddPage));
            Routing.RegisterRoute(nameof(ProblemDetailsPage), typeof(ProblemDetailsPage));
            Routing.RegisterRoute(nameof(ProblemsViewPage), typeof(ProblemsViewPage));
            Routing.RegisterRoute(nameof(DocumentAddPage), typeof(DocumentAddPage));
            Routing.RegisterRoute(nameof(DocumentDetailsPage), typeof(DocumentDetailsPage));
            Routing.RegisterRoute(nameof(DocumentViewPage), typeof(DocumentViewPage));
            Routing.RegisterRoute(nameof(MedicationAddPage), typeof(MedicationAddPage));
            Routing.RegisterRoute(nameof(MedicationDetailsPage), typeof(MedicationDetailsPage));
            Routing.RegisterRoute(nameof(MedicationViewPage), typeof(MedicationViewPage));
            Routing.RegisterRoute(nameof(LogbookAddPage), typeof(LogbookAddPage));
            Routing.RegisterRoute(nameof(LogbookEditPage), typeof(LogbookEditPage));
            Routing.RegisterRoute(nameof(AddRatingPage), typeof(AddRatingPage));
        }
    }
}