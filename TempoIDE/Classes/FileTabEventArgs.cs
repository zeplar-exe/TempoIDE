using System.Windows;
using TempoIDE.UserControls;

namespace TempoIDE.Classes
{
    public delegate void FileTabEventHandler(object sender, FileTabEventArgs e);
    
    public class FileTabEventArgs : RoutedEventArgs
    {
        public EditorTabButton TabButton;

        public FileTabEventArgs(EditorTabButton tabButton)
        {
            TabButton = tabButton;
        }
    }
}