namespace HealthProject.Views
{
    public partial class AppShell : Shell
    {
        public AppShell()
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
        }
    }
}