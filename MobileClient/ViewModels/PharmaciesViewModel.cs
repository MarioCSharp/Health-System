using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.PharmacyService;
using HealthProject.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class PharmaciesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<PharmacyDisplayModel> pharmacies;

        private readonly IPharmacyService pharmacyService;

        public PharmaciesViewModel(IPharmacyService pharmacyService)
        {
            this.pharmacyService = pharmacyService;

            CheckProductsCommand = new AsyncRelayCommand<int>(CheckProductsAsync);

            LoadPharmacies();
        }

        public IAsyncRelayCommand<int> CheckProductsCommand { get; }

        public async void LoadPharmacies()
        {
            var phars = await pharmacyService.GetAll();

            Pharmacies = new ObservableCollection<PharmacyDisplayModel>(phars);
        }

        private async Task CheckProductsAsync(int pharmacyId)
        {
            await Shell.Current.GoToAsync($"{nameof(PharmacyProductsPage)}?pharmacyId={pharmacyId}");
        }
    }
}
