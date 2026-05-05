using System.Windows;

namespace MEFExampleApp.Shell
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Bootstrapper builds the MEF container and creates the main window.
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
