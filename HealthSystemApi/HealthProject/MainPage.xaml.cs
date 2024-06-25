using HealthProject.Services;
using HealthProject.ViewModels;

namespace HealthProject
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }
    }
}
