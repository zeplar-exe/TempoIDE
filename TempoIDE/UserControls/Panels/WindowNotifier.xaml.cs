using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.UserControls.Panels
{
    public partial class WindowNotifier : Panel
    {
        public WindowNotifier()
        {
            InitializeComponent();
        }

        public void Notify(string message, NotificationIcon icon)
        {
            var notification = new WindowNotification(message, icon);
            notification.Closed += Notification_OnClosed;
            
            Children.Insert(0, notification);

            InvalidateVisual();
        }
        
        private void Notification_OnClosed(object sender, EventArgs e)
        {
            Children.Remove((WindowNotification)sender);
            
            InvalidateVisual();
        }

        public void Clear()
        {
            Children.Clear();
            
            InvalidateVisual();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var height = 0d;

            foreach (var child in Children.OfType<WindowNotification>())
            {
                height += child.Height;
                
                child.Arrange(new Rect(new Point(0, height), child.DesiredSize));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(Width, Children.OfType<WindowNotification>().Sum(child => child.Height));
        }
    }

    public enum NotificationIcon
    {
        Information,
        Warning,
        Error
    }
}