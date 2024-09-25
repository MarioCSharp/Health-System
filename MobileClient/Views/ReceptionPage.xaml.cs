using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class ReceptionPage : ContentPage
{
    private ReceptionViewModel _viewModel;

    public ReceptionPage(ReceptionViewModel viewModel)
    {
        InitializeComponent();

        Title = "Актуални рецепции";

        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        _viewModel.LoadHospitals();
        base.OnAppearing();
    }

    private void OnCallReceptionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var phoneNumber = button?.CommandParameter as string;

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            if (PhoneDialer.Default.IsSupported)
            {
                try
                {
                    PhoneDialer.Default.Open(phoneNumber);
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"Invalid phone number: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Phone dialer is not supported on this device/emulator.");
            }
        }
    }
}
