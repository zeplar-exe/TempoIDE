using System;
using System.Windows.Input;
using TempoIDE.Classes.Types;
using TempoIDE.Windows;

namespace TempoIDE.Classes.Commands
{
    public class CopyTextCommand : AppCommand
    {
        public override bool CanExecute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;

            return window.Editor.TextEditor.IsFocused || window.Explorer.IsFocused;
        }

        public override void Execute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            window.Editor.TextEditor.TryCopyText();
        }
    }
}