using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.UserControls
{
    public partial class SettingsCategoryControl : ItemsControl
    {
        public string Header
        {
            get => (string) GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header", typeof(string),
                typeof(SettingsCategoryControl)
            );
        
        public SettingsCategoryControl()
        {
            InitializeComponent();
        }
    }
}