using System.Windows;
using System.Windows.Controls;
using TempoIDE.UserControls;

namespace TempoIDE.Windows
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        
        private void SettingsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ContentDisplay.Template = GetSectionTemplate(Explorer.SelectedItem as UIElement);
        }

        public static readonly DependencyProperty SectionTemplateProperty = DependencyProperty.RegisterAttached(
            "SectionTemplate",
            typeof(ControlTemplate),
            typeof(ExplorerViewItem)
        );
        
        public static void SetSectionTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(SectionTemplateProperty, value);
        }
        
        public static ControlTemplate GetSectionTemplate(UIElement element)
        {
            return (ControlTemplate)element?.GetValue(SectionTemplateProperty);
        }

        private void Explorer_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ContentDisplay.Template = GetSectionTemplate(e.NewValue as UIElement);
        }
    }
}