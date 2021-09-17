using System;
using System.Windows;
using TempoIDE.Core.Plugins;
using TempoIDE.Core.Static;

namespace TempoIDE
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ConsoleManager.Show();
            
            #if RELEASE
            ConsoleManager.Hide();
            #endif
            
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            #if RELEASE
            ApplicationHelper.EmitErrorCode(
                ApplicationErrorCode.TI_UNHANDLED, 
                $"An unhandled exception was thrown:\n{e.ExceptionObject as Exception}");
            #endif
        }
        
        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(ApplicationHelper.ActiveWindow);
        }
    }
}