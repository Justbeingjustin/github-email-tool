using Github.EmailAddressTool.CompositionRoots;
using System.Windows;
using Unity;
namespace Github.EmailAddressTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var container = CompositionRoot.Configure();
            MainWindow mainWindow = new MainWindow();
            var mainWindowViewModel = container.Resolve<MainWindowViewModel>();
            mainWindow.DataContext = mainWindowViewModel;
            mainWindow.Show();
        }
    }
}
