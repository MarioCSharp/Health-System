using HealthProject.ViewModels;

namespace HealthProject.Views
{
    public partial class MedicationSchedulePage : ContentPage
    {
        private MedicationScheduleViewModel viewModel;

        public MedicationSchedulePage(MedicationScheduleViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.OnDateSelected(e.NewDate);
        }
    }
}
