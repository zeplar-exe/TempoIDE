using System;
using System.Windows;
using CSharp_Logger;
using TempoIDE.Core.Helpers;

namespace TempoIDE;

public partial class App
{
    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        var logger = new Logger();
        logger.SetConfiguration(IOHelper.GetRelativePath("data\\logs.log"), LogFilterFactory.AllTrue());
            
        ApplicationHelper.Logger = logger;
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