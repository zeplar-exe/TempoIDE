using System;
using System.Windows;
using TempoIDE.Core.Static;

namespace TempoIDE.Windows
{
    public partial class ErrorDialog
    {
        public ErrorDialog()
        {
            InitializeComponent();
        }

        public static void ShowError(string message)
        {
            var dialog = new ErrorDialog
            {
                ErrorBlock =
                {
                    Text = message
                },
                Owner = EnvironmentHelper.ActiveWindow
            };

            dialog.ShowDialog();
        }
        
        public static void ShowError(Exception exception)
        {
            ShowError(exception.ToString());
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}