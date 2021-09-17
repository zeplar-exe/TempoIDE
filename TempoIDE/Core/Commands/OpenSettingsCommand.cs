using TempoIDE.Core.Static;
using TempoIDE.Windows;

namespace TempoIDE.Core.Commands
{
    public class OpenSettingsCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var settingWindow = new SettingsWindow { Owner = ApplicationHelper.MainWindow };
            
            settingWindow.ShowDialog();
        }
    }
}