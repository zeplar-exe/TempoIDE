using System.Windows;
using System.Windows.Controls;
using TempoIDE.UserControls;

namespace TempoIDE.Windows
{
    public partial class CreateSolutionWindow : Window
    {
        public CreateSolutionWindow()
        {
            InitializeComponent();
        }
        
        private void CreateSolutionWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ContentDisplay.Template = GetCreationTemplate(Explorer.SelectedItem as UIElement);
        }
        
        public static readonly DependencyProperty CreationTemplateProperty = DependencyProperty.RegisterAttached(
            "CreationTemplate",
            typeof(ControlTemplate),
            typeof(ExplorerViewItem)
        );
        
        public static void SetCreationTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(CreationTemplateProperty, value);
        }
        public static ControlTemplate GetCreationTemplate(UIElement element)
        {
            return (ControlTemplate)element?.GetValue(CreationTemplateProperty);
        }

        private void Explorer_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ContentDisplay.Template = GetCreationTemplate(e.NewValue as UIElement);
        }
    }
}