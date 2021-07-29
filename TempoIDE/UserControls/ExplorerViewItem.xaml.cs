using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TempoIDE.Annotations;

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