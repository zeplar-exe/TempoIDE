using TempoIDE.Core.Static;
using TempoIDE.Windows;

namespace TempoIDE.Core.Commands
{
    public class SelectAllCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => ApplicationHelper.MainWindow.IsActive;

        public override void Execute(object parameter)
        {
            var window = ApplicationHelper.MainWindow;
            
            window.Editor.SelectedEditor.TrySelectAll();
        }
    }
}