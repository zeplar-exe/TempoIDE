using System.Windows;
using TempoIDE.UserControls;

namespace TempoIDE.Core.Types.EventArgs
{
    public delegate void OpenFileEventHandler(object sender, OpenExplorerElementArgs e);
    
    public class OpenExplorerElementArgs : RoutedEventArgs
    {
        public OpenExplorerElementArgs(ExplorerViewItem element)
        {
            Element = element;
        }

        public readonly ExplorerViewItem Element;
    }
}