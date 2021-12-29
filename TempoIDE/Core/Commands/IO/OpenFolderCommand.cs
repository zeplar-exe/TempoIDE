using Ookii.Dialogs.Wpf;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.Commands.IO;

public class OpenFolderCommand : AppCommand
{
    public override bool CanExecute(object parameter) => true;

    public override void Execute(object parameter)
    {
        var dialog = new VistaFolderBrowserDialog();
            
        if (dialog.ShowDialog() == true)
        {
            EnvironmentHelper.LoadEnvironment(dialog.SelectedPath);   
        }
    }
}