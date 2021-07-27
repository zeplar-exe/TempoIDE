namespace TempoIDE.Classes.Commands
{
    public class SelectAllTextCommand : AppCommand
    {
        public override bool CanExecute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            if (window.Editor.SelectedEditor == null)
                return false;
            
            return window.Editor.Tabs.GetSelectedItem() != null || window.Editor.IsFocused;
        }

        public override void Execute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            window.Editor.SelectedEditor.TrySelectAll();
        }
    }
}