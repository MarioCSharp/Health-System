using HealthProject.Services.DoctorService;
using HealthProject.ViewModels;

namespace HealthProject.Views;

[QueryProperty(nameof(AppointmentId), "id")]
public partial class AddRatingPage : ContentPage
{
    private int appointmentId;
    public int AppointmentId
    {
        get => appointmentId;
        set
        {
            appointmentId = value;
        }
    }
    public AddRatingPage(IDoctorService doctorService)
	{
		InitializeComponent();

        BindingContext = new AddRatingViewModel(appointmentId, doctorService);
	}
}