using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.UserControls.Panels
{
    public partial class WindowNotifier : StackPanel
    {
        public WindowNotifier()
        {
            InitializeComponent();
        }

        public void Notify(string message, NotificationIcon icon)
        {
            var notification = new WindowNotification(message, icon);
            notification.Closed += Notification_OnClosed;

            Children.Add(notification);
        }
        
        private void Notification_OnClosed(object sender, EventArgs e)
        {
            Children.Remove((WindowNotification)sender);
        }

        public void Clear()
        {
            Children.Clear();
        }
    }

    public enum NotificationIcon
    {
        Information,
        Warning,
        Error
    }
}