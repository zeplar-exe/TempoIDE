using System.IO;
using System.Windows.Controls;

namespace TempoIDE.UserControls
{
    public partial class ExplorerFileElement : ExplorerPanelElement
    {
        private string filePath;
        public string FilePath
        {
            get => filePath;
            set { filePath = value; Update(); }
        }
        private ExplorerPanelElementType type;
        
        public ExplorerFileElement()
        {
            InitializeComponent();
        }
        
        public ExplorerFileElement(string path)
        {
            InitializeComponent();

            FilePath = path;
            
            Expand();
            Refresh();
            Children.CollectionChanged += delegate { Refresh(); };
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

            Header = Path.GetFileName(filePath);

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
}