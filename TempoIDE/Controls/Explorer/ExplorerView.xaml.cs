using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Core;
using TempoIDE.Core.CustomEventArgs;

namespace TempoIDE.Controls.Explorer
{
    public partial class ExplorerView : TreeView
    {
        public Color SelectedItemColor { get; set; } = Brushes.Blue.Color;
        public Color UnfocusedItemColor { get; set; } = Brushes.CadetBlue.Color;

        public static readonly string[] SupportedExtensions =
        {
            ".txt", ".cs", ".xml", ".xaml"
        };

        public event OpenFileEventHandler OpenItemEvent;

        public ExplorerView()
        {
            InitializeComponent();
        }

        public IEnumerable<TExplorerItem> EnumerateTree<TExplorerItem>() where TExplorerItem : ExplorerItem
        {
            foreach (var item in Items.OfType<TExplorerItem>())
            {
                yield return item;
                
                foreach (var nested in item.EnumerateTree<TExplorerItem>())
                    yield return nested;
            }
        }

        public void Add(ExplorerItem item)
        {
            AddChild(item);
        }
        
        public void Clear() => Items.Clear();

        private void ExplorerView_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
                return;

            var clicked = (UIElement)e.OriginalSource;
            
            if (clicked is null)
                return;

            var element = clicked.FindAncestorOfType<ExplorerItem>();

            OpenItemEvent?.Invoke(this, new OpenExplorerElementArgs(element));
        }

        private void ExplorerView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clicked = (UIElement)e.OriginalSource;
            
            if (clicked is null)
                return;

            clicked.FindAncestorOfType<TreeViewItem>().IsSelected = true;
        }
    }
}