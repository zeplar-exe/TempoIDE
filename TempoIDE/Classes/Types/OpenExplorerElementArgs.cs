using System.IO;
using System.Windows;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.Types
{
    public delegate void OpenFileEventHandler(object sender, OpenExplorerElementArgs e);
    
    public class OpenExplorerElementArgs : RoutedEventArgs
    {
        public OpenExplorerElementArgs(ExplorerPanelElement element)
        {
            Element = element;
        }

        public ExplorerPanelElement Element;
    }
}