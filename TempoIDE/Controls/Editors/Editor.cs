using System.IO;
using System.Windows.Controls;

namespace TempoIDE.Controls.Editors
{
    public abstract class Editor : UserControl
    {
        public new abstract bool IsFocused { get; }
    }

    public abstract class FileEditor : Editor
    {
        public FileInfo BoundFile;
        
        public abstract void Refresh();
        public abstract void Update(FileInfo file);
        
        public abstract void UpdateVisual();
        public abstract void UpdateFile();

        public abstract void FileWriter();
    }
}