using System;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Core.Commands;
using TempoIDE.Core.Static;

namespace TempoIDE.UserControls.Panels
{
    public partial class FileSystemItemContextMenu : ContextMenu
    {
        private readonly ExplorerFileSystemItem fileSystemItem;
        
        public FileSystemItemContextMenu(ExplorerFileSystemItem fileSystemItem)
        {
            this.fileSystemItem = fileSystemItem;
            
            InitializeComponent();
        }
        
        public static readonly RoutedCommandExt CreateTypeFile = new();
        public static readonly RoutedCommandExt ExcludeFile = new();

        private void CreateTypeFileExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void ExcludeFileExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            EnvironmentHelper.ConfigStream.Excluded.Add(fileSystemItem.FilePath);
            EnvironmentHelper.ConfigStream.Write();
            
            EnvironmentHelper.RefreshCache();
            EnvironmentHelper.RefreshExplorer();
        }
    }
}