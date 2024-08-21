using HealthProject.Services.AuthenticationService;
using HealthProject.Services.HealthIssueService;
using HealthProject.ViewModels;

namespace HealthProject.Views;

[QueryProperty(nameof(UserId), "userId")]
public partial class HealthIssueAddPage : ContentPage
{
    private string? userId;
    private IHealthIssueService healthIssueService;
    private IAuthenticationService authenticationService;
    public string UserId
    {
        get => userId;
        set
        {
            userId = value;
            BindingContext = new HealthIssueAddViewModel(healthIssueService, userId, authenticationService);
        }
    }
    public HealthIssueAddPage(IHealthIssueService healthIssueService,
                              IAuthenticationService authenticationService)
	{
		InitializeComponent();

        Title = "Добавяне на здравен проблем";

        this.healthIssueService = healthIssueService;
        this.authenticationService = authenticationService;
	}
}