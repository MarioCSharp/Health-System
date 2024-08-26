using HealthProject.ViewModels;

namespace HealthProject.Views
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel mainPageViewModel;
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = this.mainPageViewModel = mainPageViewModel;
        }

        private double _lastScrollY = 0;
        private bool _isFooterVisible = true;

        private async void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            double currentScrollY = e.ScrollY;

            if (currentScrollY > _lastScrollY)
            {
                if (_isFooterVisible)
                {
                    _isFooterVisible = false;
                    await FooterGrid.FadeTo(0, 500); 
                }
            }
            else if (currentScrollY < _lastScrollY)
            {
                if (!_isFooterVisible)
                {
                    _isFooterVisible = true;
                    await FooterGrid.FadeTo(1, 500); 
                }
            }

            _lastScrollY = currentScrollY;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.mainPageViewModel.LoadResources();
        }
    }
}
