using System;
using System.Windows;
using CSharp_Logger;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig;

namespace TempoIDE
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ApplicationHelper.Start();
            
            InitSettings();
            SetSkin();
            InitLogger();
            HookEvents();
        }

        private void InitSettings()
        {
            SettingsHelper.Settings.Parse();
        }

        private void SetSkin()
        {
            if (!SkinHelper.TryLoadSkin(SettingsHelper.Settings.AppSettings.SkinSettings.SkinConfig.CurrentSkin))
                SkinHelper.LoadDefaultSkin();
        }

        private void InitLogger()
        {
            var logger = new Logger();
            logger.SetConfiguration(IOHelper.GetRelativePath(@"appdata\output.log"), LogFilterFactory.AllTrue());
            
            ApplicationHelper.Logger = logger;
        }

        private void HookEvents()
        {
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
        
        private void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(ApplicationHelper.ActiveWindow);
        }
    }
}