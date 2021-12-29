using TempoIDE.Core.Helpers;
using TempoIDE.Windows.SolutionCreation;

namespace TempoIDE.Core.Commands.UI;

public class NewSolutionCommand : AppCommand
{
    public override bool CanExecute(object parameter) => true;

    public override void Execute(object parameter)
    {
        var solutionWindow = new CreateSolutionWindow { Owner = ApplicationHelper.MainWindow };
            
        solutionWindow.ShowDialog();
    }
}