using System.IO;
using System.Windows;
using System.Windows.Controls;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Windows;

public partial class SettingsWindow : ModifiedWindow
{
    public SettingsWindow()
    {
        InitializeComponent();
    }
        
    private void SettingsWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        v_ContentDisplay.Template = GetSectionTemplate(v_Explorer.SelectedItem as UIElement);
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
        
    public static ControlTemplate? GetSectionTemplate(UIElement? element)
    {
        return (ControlTemplate?)element?.GetValue(SectionTemplateProperty);
    }

    private void Explorer_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        v_ContentDisplay.Template = GetSectionTemplate(e.NewValue as UIElement);
    }

    private void Skins_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = (FileInfo)((ComboBox)sender).SelectedItem;
            
        if (item == null)
            return;
            
        SkinHelper.TryLoadSkin(item.Name);
    }
}