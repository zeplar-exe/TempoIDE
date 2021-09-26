using System;
using System.Diagnostics;
using System.Windows;
using CSharp_Logger;
using TempoIDE.Core;
using TempoIDE.Core.Helpers;

namespace TempoIDE
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var logger = new Logger();
            logger.SetConfiguration(IOHelper.GetRelativePath("data\\logs.log"), LogFilterFactory.AllTrue());

            logger.CatchLog += (_, logArgs) => Console.WriteLine(logArgs.Message);
            
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
}