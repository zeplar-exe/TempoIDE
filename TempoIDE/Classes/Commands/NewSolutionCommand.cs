using System.IO;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace TempoIDE.Classes.Commands
{
    public class NewSolutionCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog().ToRealValue())
            {
                EnvironmentHelper.CreateSolution(new DirectoryInfo(dialog.SelectedPath), "solution");
            }
        }
    }
}