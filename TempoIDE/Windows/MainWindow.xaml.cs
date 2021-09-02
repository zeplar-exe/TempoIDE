﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TempoIDE.Core.Commands;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types.EventArgs;
using TempoIDE.Properties;
using TempoIDE.UserControls.Editors;
using TempoIDE.UserControls.Panels;
using TempoIDE.UserControls.SidebarControl;

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
            if (EnvironmentHelper.MainWindow.Editor.SelectedEditor == null)
                return false;

            return EnvironmentHelper.MainWindow.Editor.Tabs.GetFocusedEditor() != null || EnvironmentHelper.MainWindow.Explorer.IsFocused;
        }
        
        public void MinimizeWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            var window = EnvironmentHelper.ActiveWindow;
            
            if (window.WindowState.HasFlag(WindowState.Minimized))
                SystemCommands.MaximizeWindow(window);
            else
                SystemCommands.MinimizeWindow(EnvironmentHelper.ActiveWindow);
        }

        public void MaximizeWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            var window = EnvironmentHelper.ActiveWindow;
            
            if (window.WindowState.HasFlag(WindowState.Maximized))
                SystemCommands.MinimizeWindow(window);
            else
                SystemCommands.MaximizeWindow(EnvironmentHelper.ActiveWindow);
        }

        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(EnvironmentHelper.ActiveWindow);
        }
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadKeybindings();

            Shortcuts.Default.SettingChanging += delegate { LoadKeybindings(); };
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