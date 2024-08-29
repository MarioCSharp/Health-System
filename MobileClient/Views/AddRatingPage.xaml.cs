using HealthProject.Services.DoctorService;
using HealthProject.ViewModels;

namespace HealthProject.Views;

[QueryProperty(nameof(AppointmentId), "id")]
public partial class AddRatingPage : ContentPage
{
    private int appointmentId;
    private IDoctorService doctorService;
    private AddRatingViewModel viewModel;
    public int AppointmentId
    {
        get => appointmentId;
        set
        {
            appointmentId = value;
            if (viewModel != null)
            {
                viewModel.AppointmentId = value;
            }
        }
    }

    public AddRatingPage(IDoctorService doctorService)
	{
		InitializeComponent();
        this.doctorService = doctorService;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel = new AddRatingViewModel(appointmentId, doctorService);
        BindingContext = viewModel;
    }
}