using System.Collections.Generic;
using TempoControls.Core.Types;

namespace TempoControls.Controls
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