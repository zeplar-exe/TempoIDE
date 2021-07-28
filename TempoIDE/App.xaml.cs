using System.Windows;
using TempoCompiler.Cs;
using TempoIDE.Classes;

namespace TempoIDE
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            #if DEBUG
            ConsoleManager.Show();
            #endif
            
            //ThemeHelper.LoadTheme(Theme.Light);
            new Compiler(null);
        }

        public void MinimizeWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.MinimizeWindow(EnvironmentHelper.ActiveWindow);
        }

        public void MaximizeWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.MaximizeWindow(EnvironmentHelper.ActiveWindow);
        }

        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(EnvironmentHelper.ActiveWindow);
        }
    }
}