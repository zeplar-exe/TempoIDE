using System;
using System.Linq;
using System.Windows;
using CSharp_Logger;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Helpers.Plugins;

namespace TempoIDE
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ApplicationHelper.Start();
            
            InitSettings();
            InitPlugins();
            SetSkin();
            InitLogger();
            HookEvents();
        }

        private void InitSettings()
        {
            SettingsHelper.Start();
            SettingsHelper.MoveDirectory(AppDataHelper.Directory.CreateSubdirectory("settings"));
        }

        private void InitPlugins()
        {
            PluginsHelper.MoveDirectory(AppDataHelper.Directory.CreateSubdirectory("plugins"));

            foreach (var plugin in PluginsHelper.Plugins.Plugins.Where(p => p.Enabled))
                plugin.Start();
            
            SettingsHelper.Update();
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
        
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            SettingsHelper.Settings.Dispose();
        }
        
        private void CloseActiveWindow(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(ApplicationHelper.ActiveWindow);
        }
    }
}