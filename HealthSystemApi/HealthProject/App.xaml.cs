namespace HealthProject
{
    public partial class App : Application
    {
        public App(HomePage homePage)
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
