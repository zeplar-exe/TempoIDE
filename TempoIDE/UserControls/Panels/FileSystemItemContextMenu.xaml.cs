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
            EnvironmentHelper.Current.ConfigStream.Exclude(fileSystemItem.FilePath);
            EnvironmentHelper.Current.ConfigStream.Write();
            
            EnvironmentHelper.Current.RefreshCache();
            EnvironmentHelper.RefreshExplorer();
        }
    }
}