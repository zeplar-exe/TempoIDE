using System;
using System.Windows.Input;
using System.Windows.Shapes;

namespace TempoIDE.Windows
{
    public partial class MainWindow
    {
        private bool isResizing;

        private void Resize_Init(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle senderRect)
            {
                isResizing = true;
                senderRect.CaptureMouse();
            }
        }

        private void Resize_End(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle senderRect)
            {
                isResizing = false;

                senderRect.ReleaseMouseCapture();
            }
        }

        private void Resizing_Form(object sender, MouseEventArgs e)
        {
            if (!isResizing)
                return;

            if (sender is Rectangle senderRect && senderRect?.Tag is MainWindow mainWindow)
            {
                var width = e.GetPosition(mainWindow).X;
                var height = e.GetPosition(mainWindow).Y;
                
                senderRect.CaptureMouse();
                
                if (senderRect.Name.ToLower().Contains("right"))
                {
                    width += 5;
                    if (width > 0)
                        mainWindow.Width = width;
                }

                if (senderRect.Name.ToLower().Contains("left"))
                {
                    width -= 5;
                    mainWindow.Left += width;
                    width = mainWindow.Width - width;
                    if (width > 0) mainWindow.Width = width;
                }

                if (senderRect.Name.ToLower().Contains("bottom"))
                {
                    height += 5;
                    if (height > 0)
                        mainWindow.Height = height;
                }

                if (senderRect.Name.ToLower().Contains("top"))
                {
                    height -= 5;
                    mainWindow.Top += height;
                    height = mainWindow.Height - height;
                    if (height > 0) mainWindow.Height = height;
                }
            }
        }
    }
}