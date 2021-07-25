using Ookii.Dialogs.Wpf;
using TempoIDE.Classes.Types;

namespace TempoIDE.Classes.Commands
{
    public class OpenFolderCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var dialog = new VistaFolderBrowserDialog();
            
            if (dialog.ShowDialog().ToRealValue())
            {
                EnvironmentHelper.LoadEnvironment(dialog.SelectedPath, EnvironmentFilterMode.Directory);   
            }
        }
    }
}