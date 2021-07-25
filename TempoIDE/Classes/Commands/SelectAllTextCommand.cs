using TempoIDE.Classes.Types;
using TempoIDE.Windows;

namespace TempoIDE.Classes.Commands
{
    public class SelectAllTextCommand : AppCommand
    {
        public override bool CanExecute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            return window.Editor.TextEditor.IsFocused || window.Editor.IsFocused;
        }

        public override void Execute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            window.Editor.TextEditor.TrySelectAll();
        }
    }
}