using System.IO;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using TempoIDE.Windows;

namespace TempoIDE.Classes.Commands
{
    public class NewSolutionCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var solutionWindow = new CreateSolutionWindow { Owner = EnvironmentHelper.MainWindow };
            
            solutionWindow.ShowDialog();
        }
    }
}