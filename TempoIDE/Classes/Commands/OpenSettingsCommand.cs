using TempoIDE.Windows;

namespace TempoIDE.Classes.Commands
{
    public class OpenSettingsCommand : AppCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            var settingWindow = new SettingsWindow { Owner = EnvironmentHelper.MainWindow };
            
            settingWindow.ShowDialog();
        }
    }
}