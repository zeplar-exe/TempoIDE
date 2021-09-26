using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.Controls.SubControls
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
            InitializeComponent();
        }
    }
}