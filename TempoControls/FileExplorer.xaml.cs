using System.Collections.Generic;
using System.Windows;
using TempoControls.Core.Types;

namespace TempoControls
{
    public partial class FileExplorer
    {
        public string DirectoryPath { get; set; }
        public List<FilterExtension> Filter { get; set; }

        public FileExplorer()
        {
            InitializeComponent();
        }

        private void DirectoryPathBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            RefreshLayout();
        }

        private void RefreshLayout()
        {
            
        }
    }

    public enum DisplayMode
    {
        Browse = 0,
        FilesOnly,
        DirectoriesOnly,
        FilesAndDirectories
    }

    public enum ExplorerMode
    {
        None = 0,
        Save,
        Open
    }
}