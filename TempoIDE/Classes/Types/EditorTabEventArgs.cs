using System.IO;
using System.Windows;

namespace TempoIDE.Classes.Types
{
    public delegate void FileTabEventHandler(object sender, EditorTabEventArgs e);
    
    public class EditorTabEventArgs : RoutedEventArgs
    {
        public readonly FileInfo File;

        public EditorTabEventArgs(FileInfo file)
        {
            File = file;
        }
    }
}