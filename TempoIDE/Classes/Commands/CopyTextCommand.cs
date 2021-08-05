using TempoIDE.Windows;

namespace TempoIDE.Classes.Commands
{
    public class CopyTextCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => MainWindow.IsCoreElementFocused();

        public override void Execute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            window.Editor.SelectedEditor.TryCopy();
        }
    }
}