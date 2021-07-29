using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TempoIDE.Classes;
using TempoIDE.Classes.Commands;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

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
            
            var shortcuts = Properties.Shortcuts.Default;

            InputBindings.AddRange(new List<KeyBinding>
            {
                new KeyBinding(new CopyTextCommand(), shortcuts.Copy.ToGesture()),
                new KeyBinding(new PasteTextCommand(), shortcuts.Paste.ToGesture()),
                new KeyBinding(new CutTextCommand(), shortcuts.Cut.ToGesture()),
                new KeyBinding(new SelectAllTextCommand(), shortcuts.SelectAll.ToGesture())
            });
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Editor.SelectedEditor?.UpdateFile();
        }

        private void ExplorerPanel_OnOpenFileEvent(object sender, OpenExplorerElementArgs e)
        {
            var path = (e.Element as ExplorerFileItem)?.FilePath;
            
            if (path.Null())
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