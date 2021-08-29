using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.UserControls.MiniControls
{
    public partial class SettingsCategoryControl : ItemsControl
    {
        public string HeaderText
        {
            get => (string) GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(
                "HeaderText", typeof(string),
                typeof(SettingsCategoryControl)
            );
        
        public SettingsCategoryControl()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}