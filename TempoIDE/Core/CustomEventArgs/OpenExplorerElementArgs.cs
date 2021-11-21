using System.Windows;
using TempoIDE.Controls.Explorer;
using TempoIDE.Controls.Panels;

namespace TempoIDE.Core.CustomEventArgs
{
    public delegate void OpenFileEventHandler(object sender, OpenExplorerElementArgs e);
    
    public class OpenExplorerElementArgs : RoutedEventArgs
    {
        public ExplorerItem Element { get; }
        
        public OpenExplorerElementArgs(ExplorerItem element)
        {
            Element = element;
        }
    }
}