using Foundation;
using UIKit;

namespace HealthProject.Platforms.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //NotificationCenter.AskPermission();
            return base.FinishedLaunching(app, options);
        }
    }
}
