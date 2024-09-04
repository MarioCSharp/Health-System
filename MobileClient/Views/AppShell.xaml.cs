﻿using HealthProject.Services.AuthenticationService;

namespace HealthProject.Views
{
    public partial class AppShell : Shell
    {

        public AppShell(IAuthenticationService authenticationService)
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(HospitalDetailsPage), typeof(HospitalDetailsPage));
            Routing.RegisterRoute(nameof(DoctorDetailsPage), typeof(DoctorDetailsPage));
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
            Routing.RegisterRoute(nameof(ReceptionChatPage), typeof(ReceptionChatPage));
            Routing.RegisterRoute(nameof(RemindersAddPage), typeof(RemindersAddPage));
            Routing.RegisterRoute(nameof(PharmacyProductsPage), typeof(PharmacyProductsPage));
        }
    }
}