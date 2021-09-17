using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TempoIDE.Core.Commands;
using TempoIDE.Core.Plugins;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types.EventArgs;
using TempoIDE.Properties;
using TempoIDE.UserControls.Editors;
using TempoIDE.UserControls.Panels;

namespace TempoIDE.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public static bool IsCoreElementFocused()
        {
            if (ApplicationHelper.MainWindow.Editor.SelectedEditor == null)
                return false;

            return ApplicationHelper.MainWindow.Editor.Tabs.GetFocusedEditor() != null || ApplicationHelper.MainWindow.Explorer.IsFocused;
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

            SkinHelper.LoadSkin("Dark.xaml"); // TODO: Load skin from settings
            PluginHelper.LoadPlugins();
            
            Notifier.Notify("Hello world!", NotificationIcon.Information);
            Notifier.Notify("Hello world. 2!", NotificationIcon.Information);
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
            var path = (e.Element as ExplorerFileItem)?.FilePath;
            
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