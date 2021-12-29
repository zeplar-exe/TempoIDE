using System.IO;

namespace TempoIDE.Controls.Panels;

public class ExplorerFileSystemItem : ExplorerViewItem
{
    private string filePath;
    public string FilePath
    {
        get => filePath;
        set { filePath = value; Update(); }
    }
    private ExplorerPanelElementType type;
        
    public ExplorerFileSystemItem(string path)
    {
        InitializeComponent();
            
        FilePath = path;
        ContextMenu = new FileSystemItemContextMenu(this);
    }
        
    private void Update()
    {
        if (Directory.Exists(filePath))
        {
            type = ExplorerPanelElementType.Directory;
        }
        else if (File.Exists(filePath))
        {
            var file = new FileInfo(filePath);

            type = file.Extension switch
            {
                ".csproj" => ExplorerPanelElementType.Project,
                ".sln" => ExplorerPanelElementType.Solution,
                _ => ExplorerPanelElementType.File
            };
        }

        HeaderText = Path.GetFileName(filePath);

        switch (type)
        {
            case ExplorerPanelElementType.Solution:
            {
                Header = Path.GetFileNameWithoutExtension(filePath);
                    
                break;
            }
            case ExplorerPanelElementType.Project:
            {
                Header = Path.GetFileNameWithoutExtension(filePath);
                    
                break;
            }
            case ExplorerPanelElementType.Directory:
            {
                Header = Path.GetFileNameWithoutExtension(filePath);
                    
                break;
            }
            case ExplorerPanelElementType.File:
            {
                Header = Path.GetFileName(filePath);
                    
                break;
            }
        }
    }
}
    
public enum ExplorerPanelElementType { Solution, Project, Directory, File }