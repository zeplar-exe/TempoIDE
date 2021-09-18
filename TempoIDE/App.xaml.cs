using System;
using System.Diagnostics;
using System.Windows;
using CSharp_Logger;
using TempoIDE.Core;
using TempoIDE.Core.Static;

namespace TempoIDE
{
    public partial class App
    {
        public static Logger Logger;
        
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Logger = new Logger();
            Logger.SetConfiguration(IOHelper.GetRelativePath("data\\logs.log"), LogFilterFactory.AllTrue()); 
            
            #if DEBUG
            Process.Start("notepad", IOHelper.GetRelativePath("data\\logs.log"));
            #endif
            
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            #if RELEASE
            ApplicationHelper.EmitErrorCode(
                ApplicationErrorCode.TI_UNHANDLED, 
                $"An unhandled exception was thrown:\n{e.ExceptionObject as Exception}");
            #else
            throw (Exception)e.ExceptionObject;
            #endif
        }
        
        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(ApplicationHelper.ActiveWindow);
        }
    }
}