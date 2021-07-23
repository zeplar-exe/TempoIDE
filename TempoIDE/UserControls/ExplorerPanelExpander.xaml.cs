using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.UserControls
{
    public partial class ExplorerPanelExpander : UserControl
    {
        public ExplorerPanelExpander()
        {
            InitializeComponent();
        }

        public readonly List<UIElement> Children = new List<UIElement>();

        private void ElementExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            foreach (var element in Children)
            {
                if (element is ExplorerPanelExpander expander)
                    expander.ElementExpander_OnExpanded(this, e);

                element.Visibility = Visibility.Visible;
            }
        }

        private void ElementExpander_OnCollapsed(object sender, RoutedEventArgs e)
        {
            foreach (var element in Children)
            {
                if (element is ExplorerPanelExpander expander)
                    expander.ElementExpander_OnCollapsed(this, e);
                
                element.Visibility = Visibility.Collapsed;
            }
        }
    }
}