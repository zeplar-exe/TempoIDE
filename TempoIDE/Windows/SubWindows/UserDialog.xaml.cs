using System;
using System.Windows;
using System.Windows.Controls;
using TempoIDE.Core;

namespace TempoIDE.Windows.SubWindows
{
    public partial class UserDialog
    {
        public UserResult Result;
        
        public static readonly DependencyProperty BoundResult = DependencyProperty.RegisterAttached(
            "BoundResult",
            typeof(UserResult),
            typeof(Button)
        );

        public static void SetBoundResult(UIElement element, UserResult value)
        {
            element.SetValue(BoundResult, value);
        }

        public static UserResult GetBoundResult(UIElement element)
        {
            return (UserResult)element.GetValue(BoundResult);
        }
        
        public UserDialog(string message, UserResult options)
        {
            DataContext = this;
            InitializeComponent();

            TextDisplay.Text = message;

            foreach (var flag in options.EnumerateFlags())
            {
                switch (flag)
                {
                    case UserResult.Ok:
                        OkButton.Visibility = Visibility.Visible;
                        break;
                    case UserResult.Cancel:
                        CancelButton.Visibility = Visibility.Visible;
                        break;
                    case UserResult.Save:
                        SaveButton.Visibility = Visibility.Visible;
                        break;
                    case UserResult.Open:
                        OpenButton.Visibility = Visibility.Visible;
                        break;
                    case UserResult.Yes:
                        YesButton.Visibility = Visibility.Visible;
                        break;
                    case UserResult.No:
                        NoButton.Visibility = Visibility.Visible;
                        break;
                    case UserResult.Ignore:
                        IgnoreButton.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void ResultClickHandler(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            Result = GetBoundResult(button);
            DialogResult = true;

            Close();
        }
    }

    [Flags]
    public enum UserResult
    {
        None = 0x0,
        
        Ok = 0x1,
        Cancel = 0x2,
        Save = 0x3,
        Open = 0x4,
        Yes = 0x5,
        No = 0x6,
        Ignore = 0x7,
    }
}