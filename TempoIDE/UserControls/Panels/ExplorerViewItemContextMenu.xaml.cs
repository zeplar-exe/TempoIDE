using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Core.Commands;

namespace TempoIDE.UserControls.Panels
{
    public partial class ExplorerViewItemContextMenu : ContextMenu
    {
        public ExplorerViewItemContextMenu()
        {
            InitializeComponent();
        }
        
        public static RoutedCommandExt CreateTypeFile;
        public static RoutedCommandExt ExcludeFile;

        private void CreateTypeFileExecute(object sender, ExecutedRoutedEventArgs e)
        {
            // TODO: Work on a better eventing system   
        }

        private void ExcludeFileExecute(object sender, ExecutedRoutedEventArgs e)
        {
            
        }
    }
}