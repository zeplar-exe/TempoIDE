using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TempoIDE.UserControls
{
    public partial class TopbarControl : StackPanel
    {
        public TopbarControl()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Application.Current.MainWindow?.DragMove();
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(Application.Current.MainWindow);
        }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Application.Current.MainWindow;

            if (mainWindow?.WindowState == WindowState.Normal)
                SystemCommands.MaximizeWindow(mainWindow);
            else
                SystemCommands.RestoreWindow(mainWindow);
        }
        
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}