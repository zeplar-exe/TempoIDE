using System.IO;
using System.Linq;

namespace TempoIDE.UserControls
{
    public partial class EditorControl
    {
        public void ReloadOpenFiles()
        {
            FileSelectPanel.Children.Clear();

            foreach (var pair in openFiles)
            {
                FileSelectPanel.Children.Add(pair.Value);
            }

            if (openFiles.Count == 0)
            {
                TextEditor.Document.Blocks.Clear();
                TextEditor.IsReadOnly = true;
            }
            else
            {
                TextEditor.IsReadOnly = false;
            }
        }

        public void OpenFile(FileInfo file)
        {
            TextWriter();

            openFileInfo = file;

            if (openFileInfo != null)
            {
                openFileInfo.Refresh();

                bool containsFile = openFiles.Any(pair => pair.Key.FullName == openFileInfo.FullName);

                if (!containsFile)
                {
                    var fileButton = new EditorTabButton();

                    fileButton.TabButton.Content = file.Name;
                    fileButton.Resources["FileInfo"] = openFileInfo;
                    fileButton.OnButtonClicked += FileButton_OnClick;
                    fileButton.OnCloseClicked += FileClose_OnClick;

                    openFiles.Add(openFileInfo, fileButton);
                    ReloadOpenFiles();
                }
            }

            TextEditor.Document.Blocks.Clear();

            string text = file != null
                ? new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd()
                : string.Empty;
            TextEditor.AppendText(text);
            TextEditor.IsReadOnly = file == null;
            CurrentFileNameDisplay.Text = file?.FullName;
        }

        public void CloseFile(FileInfo file)
        {
            TextWriter();
            openFiles.Remove(file);
            ReloadOpenFiles();
        }

        private void FileButton_OnClick(object sender, FileTabEventArgs e)
        {
            OpenFile((FileInfo) e.TabButton.Resources["FileInfo"]);
        }

        private void FileClose_OnClick(object sender, FileTabEventArgs e)
        {
            int index = openFiles.IndexOf((FileInfo) e.TabButton.Resources["FileInfo"]);
            int nextIndex = index;
            int lastIndex = index - 1;

            TextWriter();
            CloseFile((FileInfo) e.TabButton.Resources["FileInfo"]);

            if (index == 0)
            {
                if (nextIndex < openFiles.Count)
                    OpenFile((FileInfo) openFiles[nextIndex].Value.Resources["FileInfo"]);
            }
            else
            {
                OpenFile((FileInfo) openFiles[lastIndex].Value.Resources["FileInfo"]);
            }
        }

        private void ExplorerPanel_OnOpenFileEvent(object sender, OpenFileEventArgs e)
        {
            Dispatcher.Invoke(() => { OpenFile(e.NewFile); });
        }
    }
}