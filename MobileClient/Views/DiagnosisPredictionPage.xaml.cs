namespace HealthProject.Views
{
    public partial class DiagnosisPredictionPage : ContentPage 
    {
        public DiagnosisPredictionPage(DiagnosisViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;  
        }
    }
}
