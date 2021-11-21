using System.Windows;
using System.Windows.Controls;
using TempoIDE.Controls.Explorer;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Windows
{
    public partial class SettingsWindow : ModifiedWindow
    {
        public SettingsViewModel ViewModel { get; } = new();
        
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
            typeof(ExplorerItem)
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

        private void Skins_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (SkinDefinition)((ComboBox)sender).SelectedItem;
        
            if (item == null)
                return;
            
            SkinHelper.TryLoadSkin(item.Name);
        }
    }
}