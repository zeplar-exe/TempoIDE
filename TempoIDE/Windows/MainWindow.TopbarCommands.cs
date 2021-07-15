using System;
using System.Windows.Input;
using TempoIDE.Classes;

namespace TempoIDE.Windows
{
    public partial class MainWindow
    {
        private void Topbar_Copy(object sender, MouseButtonEventArgs e)
        {
            AppCommands.Copy(this);
        }

        private void Topbar_Paste(object sender, MouseButtonEventArgs e)
        {
            AppCommands.Paste(this);
        }

        private void Topbar_Cut(object sender, MouseButtonEventArgs e)
        {
            AppCommands.Cut(this);
        }
        
        private void Topbar_SelectAll(object sender, MouseButtonEventArgs e)
        {
            AppCommands.SelectAll(this);
        }
    }
}