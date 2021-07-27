using System.Windows;
using TempoCompiler.Cs;
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
            
            //ThemeHelper.LoadTheme(Theme.Light);
            new Compiler(null);
            
            ResourceCache.Load();
        }
    }
}