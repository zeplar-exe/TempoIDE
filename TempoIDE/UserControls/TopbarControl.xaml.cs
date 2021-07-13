using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Windows;

namespace TempoIDE.UserControls
{
    public partial class TopbarControl : StackPanel
    {
        private MainWindow mainWindow;

        public TopbarControl()
        {
            InitializeComponent();
        }
        
        private void TopbarControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Application.Current.MainWindow as MainWindow;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}