using System;
using System.Windows.Data;
using TempoIDE.Classes.Types;
using TempoIDE.Windows;

namespace TempoIDE.Classes.Commands
{
    public class PasteTextCommand : AppCommand
    {
        public override bool CanExecute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            return window.Editor.TextEditor.IsFocused || window.Explorer.IsFocused;
        }

        public override void Execute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            window.Editor.TextEditor.TryPasteText();
        }
    }
}