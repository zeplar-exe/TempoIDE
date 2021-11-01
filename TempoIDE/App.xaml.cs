using System;
using System.Windows;
using CSharp_Logger;
using TempoIDE.Core.Helpers;
using TempoIDE.Properties;

namespace TempoIDE
{
    public partial class App
    {
        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            var logger = new Logger();
            logger.SetConfiguration(IOHelper.GetRelativePath(@"data\output.log"), LogFilterFactory.AllTrue());
            
            ApplicationHelper.Logger = logger;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            
            await AsyncHelper.WaitUntilNotNull(() => MainWindow);
            
            if (!SkinHelper.TryLoadSkin(SettingsHelper.Settings.AppSettings.SkinSettings.SkinConfig.CurrentSkin))
                SkinHelper.LoadDefaultSkin();
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