using HealthProject.Models;
using HealthProject.Services.LogbookService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(LogJson), "logJson")]
public partial class LogbookEditPage : ContentPage
{
    private string logJson;
    private ILogbookService logbookService;
    public string LogJson
    {
        get => logJson;
        set
        {
            logJson = value;
            var log = JsonConvert.DeserializeObject<LogAddModel>(Uri.UnescapeDataString(value));
            BindingContext = new LogbookEditViewModel(log, logbookService);
        }
    }
    public LogbookEditPage(ILogbookService logbookService)
	{
		InitializeComponent();

        this.logbookService = logbookService;
	}
}