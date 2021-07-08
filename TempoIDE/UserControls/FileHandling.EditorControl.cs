using System.IO;
using System.Linq;
using System.Threading;
using TempoIDE.Classes;

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
                TextEditor.Clear();
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
                
                TextEditor.SetScheme(openFileInfo.Extension);
            }

            TextEditor.Clear();

            string text = file == null
                ? string.Empty
                : new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd();
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
            
            if (writerThread.IsAlive)
                writerThread.Interrupt();
            
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
        
        private const int WriterCooldown = 5;

        private void TextWriterThread()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(WriterCooldown * 1000);
                }
                catch (ThreadInterruptedException)
                {
                    
                }
                
                Dispatcher.Invoke(TextWriter);
            }
        }

        internal void TextWriter()
        {
            if (openFileInfo == null) 
                return;
                    
            openFileInfo.Refresh();

            if (!textChangedBeforeUpdate)
            {
                var reader = new StreamReader(new FileStream(openFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                var text = reader.ReadToEnd();

                if (text == TextEditor.Text)
                    return;
                    
                SkipTextChange(delegate
                {
                    TextEditor.AppendText(reader.ReadToEnd()); 
                });
                
                reader.Close();
            }
            else
            {
                using var stream = new FileStream(openFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                using var writer = new StreamWriter(stream);
                stream.SetLength(0);
                writer.Write(TextEditor.Text);
            }

            textChangedBeforeUpdate = false;
        }
    }
}