using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TempoIDE.UserControls
{
    public partial class ExplorerPanelElement : UserControl
    {
        private string filePath;
        public string FilePath
        {
            get => filePath;
            set { filePath = value; Update(); }
        }
        private ExplorerPanelElementType type;

        public ExplorerPanelElement()
        {
            InitializeComponent();
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

            Text.Text = Path.GetFileName(filePath);

            switch (type)
            {
                case ExplorerPanelElementType.Solution:
                {
                    Text.Text = Path.GetFileNameWithoutExtension(filePath);
                    
                    break;
                }
                case ExplorerPanelElementType.Project:
                {
                    Text.Text = Path.GetFileNameWithoutExtension(filePath);
                    
                    break;
                }
                case ExplorerPanelElementType.Directory:
                {
                    Text.Text = Path.GetFileNameWithoutExtension(filePath);
                    
                    break;
                }
                case ExplorerPanelElementType.File:
                {
                    Text.Text = Path.GetFileName(filePath);
                    
                    break;
                }
            }
        }
    }
    
    public enum ExplorerPanelElementType { Solution, Project, Directory, File }
}