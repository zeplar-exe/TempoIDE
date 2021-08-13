using Microsoft.Win32;
using TempoIDE.Core.Static;

namespace TempoIDE.Core.Commands
{
    public class OpenFileCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog().ToRealValue())
            {
                EnvironmentHelper.LoadEnvironment(dialog.FileName);   
            }
        }
    }
}