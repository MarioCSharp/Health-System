using HealthProject.Services.DoctorService;

namespace HealthProject
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public App(HomePage homePage)
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            MainPage = new AppShell();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<HospitalDetailsPage>();
        }
    }
}
