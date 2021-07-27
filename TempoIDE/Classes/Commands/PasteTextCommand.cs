namespace TempoIDE.Classes.Commands
{
    public class PasteTextCommand : AppCommand
    {
        public override bool CanExecute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            if (window.Editor.SelectedEditor == null)
                return false;
            
            return window.Editor.Tabs.GetFocusedEditor() != null || window.Explorer.IsFocused;
        }

        public override void Execute(object parameter)
        {
            var window = EnvironmentHelper.MainWindow;
            
            window.Editor.SelectedEditor.TryPaste();
        }
    }
}