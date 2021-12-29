using System.Windows;
using TempoIDE.Controls.Panels;

namespace TempoIDE.Core.CustomEventArgs;

public delegate void OpenFileEventHandler(object sender, OpenExplorerElementArgs e);
    
public class OpenExplorerElementArgs : RoutedEventArgs
{
    public OpenExplorerElementArgs(ExplorerViewItem element)
    {
        Element = element;
    }

    public readonly ExplorerViewItem Element;
}