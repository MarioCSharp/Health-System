using HealthProject.Services.AuthenticationService;
using HealthProject.Services.DoctorService;
using HealthProject.Services.ServiceService;

namespace HealthProject.Views
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        private IAuthenticationService authenticationService;
        public App(HomePage homePage, IAuthenticationService authenticationService)
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            this.authenticationService = authenticationService;

            MainPage = new AppShell(authenticationService);
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
