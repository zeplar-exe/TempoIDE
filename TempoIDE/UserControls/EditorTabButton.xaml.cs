using System.Windows;
using System.Windows.Controls;
using TempoIDE.Classes;
using TempoIDE.Classes.Types;

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