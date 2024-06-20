
namespace HealthSystem;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
        if (this.MainPage == null)
        {
            this.MainPage = new AppShell();
        }

        return base.CreateWindow(activationState);
    }
}
