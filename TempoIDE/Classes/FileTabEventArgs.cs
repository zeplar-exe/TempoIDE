using System.Windows;

namespace TempoIDE.UserControls
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