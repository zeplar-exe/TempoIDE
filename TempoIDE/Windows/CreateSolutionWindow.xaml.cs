using System;
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
            ContentDisplay.Template = GetProjectCreationTemplate(Explorer.SelectedItem as UIElement);
        }
        
        public static readonly DependencyProperty ProjectCreationTemplateProperty = DependencyProperty.RegisterAttached(
            "ProjectCreationTemplate",
            typeof(ControlTemplate),
            typeof(ExplorerViewItem)
        );
        
        public static void SetProjectCreationTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ProjectCreationTemplateProperty, value);
        }
        public static ControlTemplate GetProjectCreationTemplate(UIElement element)
        {
            return (ControlTemplate)element?.GetValue(ProjectCreationTemplateProperty);
        }

        private void Explorer_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ContentDisplay.Template = GetProjectCreationTemplate(e.NewValue as UIElement);
        }
    }
}