using System.Windows;
using System.Windows.Input;
using TempoIDE.Classes;

namespace TempoIDE
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            #if DEBUG
            ConsoleManager.Show();
            #endif
        }
    }
}