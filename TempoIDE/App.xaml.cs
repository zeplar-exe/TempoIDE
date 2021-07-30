using System.Windows;
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
        }

        public void MinimizeWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            var window = EnvironmentHelper.ActiveWindow;
            
            if (window.WindowState.HasFlag(WindowState.Minimized))
                SystemCommands.MaximizeWindow(window);
            else
                SystemCommands.MinimizeWindow(EnvironmentHelper.ActiveWindow);
        }

        public void MaximizeWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            var window = EnvironmentHelper.ActiveWindow;
            
            if (window.WindowState.HasFlag(WindowState.Maximized))
                SystemCommands.MinimizeWindow(window);
            else
                SystemCommands.MaximizeWindow(EnvironmentHelper.ActiveWindow);
        }

        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(EnvironmentHelper.ActiveWindow);
        }
    }
}