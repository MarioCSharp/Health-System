using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.DoctorService;
using HealthProject.Views;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class AddRatingViewModel : ObservableObject
    {
        [ObservableProperty]
        private int appointmentId;

        [ObservableProperty]
        private RatingAddModel rating;

        private IDoctorService doctorService;

        public AddRatingViewModel(int appointmentId, IDoctorService doctorService)
        {
            this.doctorService = doctorService;
            this.appointmentId = appointmentId;
            Rating = new RatingAddModel();
            AddRatingCommand = new AsyncRelayCommand(AddRatingAsync);
        }

        public ICommand AddRatingCommand { get; }

        public async Task AddRatingAsync()
        {
            var result = await doctorService.AddRating(Rating, appointmentId);

            if (result)
            {
                await Shell.Current.GoToAsync($"//{nameof(AppointmentHistoryPage)}");
                return;
            }
        }
    }
}
