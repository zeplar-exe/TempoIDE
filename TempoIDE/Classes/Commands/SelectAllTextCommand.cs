namespace TempoIDE.Classes.Commands
{
    public class SelectAllTextCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => EnvironmentHelper.IsCoreElementFocused();

        public override void Execute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            window.Editor.SelectedEditor.TrySelectAll();
        }
    }
}