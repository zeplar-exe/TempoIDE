using System.Collections.Generic;

namespace TempoExplorer
{
    public partial class FileExplorer
    {
        public List<FilterExtension> Filter { get; set; }

        public FileExplorer()
        {
            InitializeComponent();
        }
    }

    public enum FileMode
    {
        FilesOnly,
        DirectoriesOnly,
        FilesAndDirectories
    }
}