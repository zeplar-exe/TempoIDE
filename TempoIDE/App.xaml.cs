using System.Windows;
using TempoIDE.Core.Static;

namespace TempoIDE
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ConsoleManager.Show();
            
            #if !DEBUG
            ConsoleManager.Hide();
            #endif
        }
        
        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(EnvironmentHelper.ActiveWindow);
        }
    }
}