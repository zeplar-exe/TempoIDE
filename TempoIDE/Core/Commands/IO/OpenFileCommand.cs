using Microsoft.Win32;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.Commands.IO
{
    public class OpenFileCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                EnvironmentHelper.LoadEnvironment(dialog.FileName);   
            }
        }
    }
}