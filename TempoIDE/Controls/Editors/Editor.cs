using System.IO;
using System.Windows.Controls;

namespace TempoIDE.Controls.Editors
{
    public abstract class Editor : UserControl
    {
        public FileInfo BoundFile;

        public new abstract bool IsFocused { get; }


        public virtual bool TryCopy() => false;
        public virtual bool TryPaste() => false;
        public virtual bool TryCut() => false;
        public virtual bool TrySelectAll() => false;
    }

    public abstract class FileEditor : Editor
    {
        public abstract void Refresh();
        public abstract void Update(FileInfo file);
        
        public abstract void UpdateVisual();
        public abstract void UpdateFile();

        public abstract void TextWriter();
    }
}