using System.IO;
using System.Windows;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.Types
{
    public delegate void OpenFileEventHandler(object sender, OpenExplorerElementArgs e);
    
    public class OpenExplorerElementArgs : RoutedEventArgs
    {
        public OpenExplorerElementArgs(ExplorerViewItem element)
        {
            Element = element;
        }

        public ExplorerViewItem Element;
    }
}