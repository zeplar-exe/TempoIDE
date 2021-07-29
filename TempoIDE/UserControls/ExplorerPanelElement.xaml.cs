using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TempoIDE.Annotations;

namespace TempoIDE.UserControls
{
    public partial class ExplorerPanelElement : UserControl
    {
        public bool IsExpanded { get; private set; }
        public readonly ObservableCollection<ExplorerPanelElement> Children = new ObservableCollection<ExplorerPanelElement>();

        public string Header
        {
            get => (string) GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header", typeof(string),
                typeof(ExplorerPanelElement)
            );
        
        public ExplorerPanelElement()
        {
            InitializeComponent();

            Text.Text = Header;
            
            Expand();
            Refresh();
            
            Children.CollectionChanged += delegate { Refresh(); };
        }

        private void ExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;

            Refresh();
        }

        public void Refresh()
        {
            if (Children.Count == 0)
                ExpandButton.Visibility = Visibility.Collapsed;
            else
                ExpandButton.Visibility = Visibility.Visible;
            
            if (IsExpanded)
                Expand();
            else
                Collapse();
        }

        public void Expand()
        {
            IsExpanded = true;
            
            foreach (var element in Children)
            {
                element.Expand();
                element.Visibility = Visibility.Visible;
            }
        }

        public void Collapse()
        {
            IsExpanded = false;

            foreach (var element in Children)
            {
                element.Collapse();
                element.Visibility = Visibility.Collapsed;
            }
        }
    }
}