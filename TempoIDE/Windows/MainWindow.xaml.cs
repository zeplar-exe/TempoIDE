using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TempoIDE.Controls.Editors;
using TempoIDE.Controls.Explorer;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.CustomEventArgs;
using TempoIDE.Core.Helpers;
using TempoIDE.Windows.SubWindows;

namespace TempoIDE.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Notify(string message, NotificationIcon icon)
        {
            Notifier.Notify(message, icon);
        }
        
        public UserResult Alert(string message, UserResult options)
        {
            var window = new UserDialog(message, options);
            window.ShowDialog();
            
            return window.Result;
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
        
        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            if (Editor.SelectedEditor is FileEditor fileEditor)
                fileEditor.FileWriter();
        }

        private void ExplorerPanel_OnOpenFileEvent(object sender, OpenExplorerElementArgs e)
        {
            var path = (e.Element as ExplorerFile)?.FilePath;
            
            if (path == null)
                return;
                
            Editor.Tabs.OpenFile(new FileInfo(path));
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