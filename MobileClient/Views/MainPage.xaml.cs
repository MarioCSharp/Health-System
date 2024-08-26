using HealthProject.ViewModels;

namespace HealthProject.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
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

            // Update last scroll position
            _lastScrollY = currentScrollY;
        }
    }
}
