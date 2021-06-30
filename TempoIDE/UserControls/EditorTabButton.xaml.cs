using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.UserControls
{
    public partial class EditorTabButton : UserControl
    {
        public FileTabEventHandler OnButtonClicked;
        public FileTabEventHandler OnCloseClicked;

        public EditorTabButton()
        {
            InitializeComponent();
        }

        private void ButtonClicked(object sender, RoutedEventArgs e) => OnButtonClicked?.Invoke(this, new FileTabEventArgs(this));
        private void CloseClicked(object sender, RoutedEventArgs e) => OnCloseClicked?.Invoke(this, new FileTabEventArgs(this));
    }
}