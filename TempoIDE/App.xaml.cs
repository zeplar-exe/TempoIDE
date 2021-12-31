using System;
using System.Windows;
using TempoIDE.Core.Helpers;

namespace TempoIDE;

public partial class App
{
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }
        
    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ApplicationHelper.EmitErrorCode(
            ApplicationErrorCode.TI_UNHANDLED, 
            $"An unhandled exception was thrown:\n{e.ExceptionObject as Exception}");
    }

    public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
    {
        SystemCommands.CloseWindow(ApplicationHelper.ActiveWindow);
    }
}