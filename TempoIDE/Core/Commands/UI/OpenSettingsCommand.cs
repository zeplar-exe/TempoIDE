using TempoIDE.Core.Helpers;
using TempoIDE.Windows;

namespace TempoIDE.Core.Commands.UI;

public class OpenSettingsCommand : AppCommand
{
    public override bool CanExecute(object parameter) => true;

    public override void Execute(object parameter)
    {
        var settingWindow = new SettingsWindow { Owner = ApplicationHelper.MainWindow };
            
        settingWindow.ShowDialog();
    }
}