using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TempoIDE.Classes.Commands;
using TempoIDE.Classes.Types;

namespace TempoIDE.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadKeybindings();

            Properties.Shortcuts.Default.SettingChanging += delegate { LoadKeybindings(); };
        }

        private void LoadKeybindings()
        {
            InputBindings.Clear();
            
            InputBindings.AddRange(new List<KeyBinding>
            {
                new KeyBinding(new CopyTextCommand(), Key.C, ModifierKeys.Control),
                new KeyBinding(new PasteTextCommand(), Key.V, ModifierKeys.Control),
                new KeyBinding(new CutTextCommand(), Key.X, ModifierKeys.Control),
                new KeyBinding(new SelectAllTextCommand(), Key.A, ModifierKeys.Control)
            });
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Editor.TextWriter();
        }

        private void ExplorerPanel_OnOpenFileEvent(object sender, OpenFileEventArgs e)
        {
            Editor.OpenFile(e.NewFile);
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