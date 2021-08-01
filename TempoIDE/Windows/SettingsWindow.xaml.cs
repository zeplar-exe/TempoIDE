using System.Windows;
using System.Windows.Controls;
using TempoIDE.Classes;
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
            var template = GetSectionTemplate(Explorer.SelectedItem as UIElement);
            
            if (template == null)
                return;
            
            ContentDisplay.Template = template;
        }

        public void CloseWindow(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(EnvironmentHelper.ActiveWindow);
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
            var template = GetSectionTemplate(e.NewValue as UIElement);
            
            if (template == null)
                return;
            
            ContentDisplay.Template = template;
        }
    }
}