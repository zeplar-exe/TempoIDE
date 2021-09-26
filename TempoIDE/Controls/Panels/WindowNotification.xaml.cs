using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TempoIDE.Controls.Panels
{
    public partial class WindowNotification
    {
        public event EventHandler Closed;
        
        public WindowNotification(string message, NotificationIcon icon)
        {
            InitializeComponent();
            
            Details.Text = message;
            Icon.Source = icon switch
            {
                NotificationIcon.Information => new BitmapImage(),
                NotificationIcon.Warning => new BitmapImage(),
                NotificationIcon.Error => new BitmapImage(),
                _ => new BitmapImage()
            };
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}