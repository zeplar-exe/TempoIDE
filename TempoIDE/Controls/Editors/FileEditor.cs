using System.IO;

namespace TempoIDE.Controls.Editors;

public abstract class FileEditor : Editor
{
    public FileInfo? BoundFile;
        
    public abstract void Refresh();
    public abstract void Update(FileInfo file);
        
    public abstract void UpdateVisual();
    public abstract void UpdateFile();

    public abstract void FileWriter();
}