using System;
using System.Windows;
using TempoIDE.Classes;

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
            
            //ThemeHelper.LoadTheme(Theme.Light);
        }
        
        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(EnvironmentHelper.ActiveWindow);
        }
    }
}