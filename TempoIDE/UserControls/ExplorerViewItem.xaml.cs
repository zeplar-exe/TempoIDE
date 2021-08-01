using System.Windows.Controls;

namespace TempoIDE.UserControls
{
    public partial class ExplorerViewItem : TreeViewItem
    {
        public string HeaderText { get; set; }
        
        public ExplorerViewItem()
        {
            InitializeComponent();
        }
    }
}