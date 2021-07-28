using System.IO;
using System.Windows.Input;

namespace TempoIDE.UserControls
{
    public partial class EditorControl
    {
        public Editor SelectedEditor => Tabs.SelectedItem?.Editor;

        private void FileClose_OnClick(object sender, MouseButtonEventArgs e)
        {
            /*var tabItem = (EditorTabItem)sender;
            
            var index = openFiles.IndexOf(tabItem.BoundFile);
            var nextIndex = index;
            var lastIndex = index - 1;

            //CloseFile(tabItem.BoundFile);

            if (index == 0)
            {
                if (nextIndex < openFiles.Count)
                    Tabs.Open(openFiles[nextIndex]);
            }
            else
            {
                Tabs.Open(openFiles[lastIndex]);
            }*/
        }
    }
}