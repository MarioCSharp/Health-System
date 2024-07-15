using HealthProject.Services.DoctorService;
using HealthProject.Services.ServiceService;

namespace HealthProject.Views
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
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<HospitalDetailsPage>();
            services.AddTransient<DoctorDetailsPage>();
        }
    }
}
