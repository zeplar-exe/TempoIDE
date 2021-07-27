using System.IO;
using System.Windows.Controls;

namespace TempoIDE.UserControls
{
    public abstract class Editor : UserControl
    {
        public FileInfo BoundFile;

        public new abstract bool IsFocused { get; }
        
        public abstract void Refresh();
        public abstract void Update(FileInfo file);

        public abstract void UpdateText();
        public abstract void UpdateFile();

        public abstract void TextWriter();


        public virtual bool TryCopy() => false;
        public virtual bool TryPaste() => false;
        public virtual bool TryCut() => false;
        public virtual bool TrySelectAll() => false;

        public static Editor FromExtension(string extension)
        {
            switch (extension.Replace(".", ""))
            {
                default:
                    return new FileEditor();
            }
        }
    }
}