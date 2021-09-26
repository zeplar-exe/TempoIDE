using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TempoIDE.Controls.Editors;
using TempoIDE.Controls.Panels;
using TempoIDE.Core;
using TempoIDE.Core.Commands;
using TempoIDE.Core.Commands.Common;
using TempoIDE.Core.CustomEventArgs;
using TempoIDE.Core.Helpers;
using TempoIDE.Plugins;
using TempoIDE.Properties;

namespace TempoIDE.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public void MinimizeWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            var window = ApplicationHelper.ActiveWindow;
            
            if (window.WindowState.HasFlag(WindowState.Minimized))
                SystemCommands.MaximizeWindow(window);
            else
                SystemCommands.MinimizeWindow(ApplicationHelper.ActiveWindow);
        }

        public void MaximizeWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            var window = ApplicationHelper.ActiveWindow;
            
            if (window.WindowState.HasFlag(WindowState.Maximized))
                SystemCommands.MinimizeWindow(window);
            else
                SystemCommands.MaximizeWindow(ApplicationHelper.ActiveWindow);
        }

        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(ApplicationHelper.ActiveWindow);
        }
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadKeybindings();
            
            Shortcuts.Default.SettingChanging += delegate { LoadKeybindings(); };
            
            if (string.IsNullOrWhiteSpace(Settings.Default.ApplicationSkin))
                SkinHelper.LoadDefaultSkin();
            else if (!SkinHelper.TryLoadSkin(Settings.Default.ApplicationSkin))
                SkinHelper.LoadDefaultSkin();
            
            PluginHelper.LoadPlugins();
        }

        private void LoadKeybindings()
        {
            InputBindings.Clear();
            
            var shortcuts = Shortcuts.Default;
            
            InputBindings.AddRange(new List<KeyBinding>
            {
                new(new CopyCommand(), shortcuts.Copy.ToGesture()),
                new(new PasteCommand(), shortcuts.Paste.ToGesture()),
                new(new CutCommand(), shortcuts.Cut.ToGesture()),
                new(new SelectAllCommand(), shortcuts.SelectAll.ToGesture())
            });
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            if (Editor.SelectedEditor is FileEditor fileEditor)
                fileEditor.TextWriter();
        }

        private void ExplorerPanel_OnOpenFileEvent(object sender, OpenExplorerElementArgs e)
        {
            var path = (e.Element as ExplorerFileSystemItem)?.FilePath;
            
            if (path == null)
                return;
                
            Editor.Tabs.Open(new FileInfo(path));
        }

        private void Editor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }

        private void Explorer_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }
    }
}