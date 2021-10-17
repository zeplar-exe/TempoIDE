using System.Windows.Controls;
using TempoIDE.Controls.Editors;

namespace TempoIDE.Controls.Panels
{
    public partial class EditorControl : Grid
    {
        public Editor SelectedEditor => Tabs.SelectedTab?.Editor;
        
        public EditorControl()
        {
            InitializeComponent();
        }
    }
}