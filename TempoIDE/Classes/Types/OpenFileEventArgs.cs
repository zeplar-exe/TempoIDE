using System.IO;
using System.Windows;

namespace TempoIDE.Classes.Types
{
    public delegate void OpenFileEventHandler(object sender, OpenFileEventArgs e);
    
    public class OpenFileEventArgs : RoutedEventArgs
    {
        public OpenFileEventArgs(FileInfo file) => NewFile = file;
        public FileInfo NewFile;
    }
}