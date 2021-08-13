using System.Windows;

namespace TempoIDE.Core.Commands
{
    public class ExitCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}