using HealthProject.Services.HealthIssueService;
using HealthProject.ViewModels;

namespace HealthProject.Views;

[QueryProperty(nameof(UserId), "userId")]
public partial class HealthIssueAddPage : ContentPage
{
    private string? userId;
    private IHealthIssueService healthIssueService;
    public string UserId
    {
        get => userId;
        set
        {
            userId = value;
            BindingContext = new HealthIssueAddViewModel(healthIssueService, userId);
        }
    }
    public HealthIssueAddPage(IHealthIssueService healthIssueService)
	{
		InitializeComponent();

        Title = "Добавяне на здравен проблем";

        this.healthIssueService = healthIssueService;
	}
}