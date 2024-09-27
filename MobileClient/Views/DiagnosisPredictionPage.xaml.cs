namespace HealthProject.Views
{
    public partial class DiagnosisPredictionPage : ContentPage 
    {
        public DiagnosisPredictionPage(DiagnosisViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;  
        }

        private async void OnMedicineButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(MedicationViewPage)}");
        }

        private async void OnRecordsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LogbookViewPage)}");
        }

        private async void OnDocumentsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(DocumentViewPage)}");
        }

        private async void OnPredictorButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(DiagnosisPredictionPage)}");
        }
    }
}
