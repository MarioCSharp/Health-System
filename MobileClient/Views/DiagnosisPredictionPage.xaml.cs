namespace HealthProject.Views
{
    public partial class DiagnosisPredictionPage : ContentPage 
    {
        public DiagnosisPredictionPage(DiagnosisViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;  
        }

        private async void OnPharmaciesButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(PharmaciesViewPage)}");
        }

        private async void OnLaboratoryButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LaboratoryPage)}");
        }

        private async void OnHospitalsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        private async void OnDiagnosisButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(DiagnosisPredictionPage)}");
        }
    }
}
