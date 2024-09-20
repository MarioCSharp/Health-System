using HealthProject.Services.MapsService;
using HealthProject.ViewModels;
using System.Windows.Input;

namespace HealthProject.Views;

public partial class HomePage : ContentPage
{
    private HomePageViewModel viewModel;

    public ICommand OpenMapsCommand { get; }

    public HomePage(HomePageViewModel homePageViewModel)
    {
        InitializeComponent();
        BindingContext = viewModel = homePageViewModel;

        OpenMapsCommand = new Command<string>(async (address) => await OpenGoogleMaps(address));

        Title = "Клиники";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadHospitals();
    }

    public async Task OpenGoogleMaps(string address)
    {
        var mapsService = new MapsService();

        if (!string.IsNullOrEmpty(address))
        {
            await mapsService.OpenGoogleMaps(address);
        }
        else
        {
            await DisplayAlert("Грешка!", "Няма намерена локация", "OK");
        }
    }
}
