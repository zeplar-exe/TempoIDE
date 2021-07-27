using System.IO;
using System.Windows.Controls;

namespace TempoIDE.UserControls
{
    public partial class EditorTabItem : ListBoxItem
    {
        public Editor Editor;
        public FileInfo BoundFile;
        
        public EditorTabItem()
        {
            InitializeComponent();
        }
    }
}