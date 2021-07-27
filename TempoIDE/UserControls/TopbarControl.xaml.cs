using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Classes;

namespace TempoIDE.UserControls
{
    public partial class TopbarControl : Border
    {
        public TopbarControl()
        {
            InitializeComponent();
        }
        
        private void Topbar_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EnvironmentHelper.MainWindow.DragMove();
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(EnvironmentHelper.MainWindow);
        }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (EnvironmentHelper.MainWindow.WindowState == WindowState.Normal)
                SystemCommands.MaximizeWindow(EnvironmentHelper.MainWindow);
            else
                SystemCommands.RestoreWindow(EnvironmentHelper.MainWindow);
        }
        
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}