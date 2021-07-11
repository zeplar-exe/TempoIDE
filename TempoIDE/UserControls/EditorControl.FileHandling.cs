using System;
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
            UpdateText();

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
                
                TextEditor.TextArea.SetScheme(openFileInfo.Extension);
            }

            TextEditor.Clear();

            string text = file == null
                ? string.Empty
                : new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd();
            TextEditor.AppendTextAtCaret(text);
            TextEditor.IsReadOnly = file == null;
            CurrentFileNameDisplay.Text = file?.FullName;
        }

        public void CloseFile(FileInfo file)
        {
            UpdateText();
            openFiles.Remove(file);
            ReloadOpenFiles();
            TextEditor.TextArea.SetScheme(null);
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
        
        private const int WriterCooldown = 2;

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
                    return;
                }
                
                Dispatcher.Invoke(TextWriter);
            }
        }

        internal void TextWriter()
        {
            if (!textChangedBeforeUpdate)
            {
                UpdateText();
            }
            else
            {
                UpdateFile();
            }

            textChangedBeforeUpdate = false;
        }

        private void UpdateText()
        {
            if (openFileInfo == null) 
                return;
                    
            openFileInfo.Refresh();
            
            var reader = new StreamReader(new FileStream(openFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            var text = reader.ReadToEnd();

            if (text == TextEditor.TextArea.Text)
                return;
                    
            SkipTextChange(delegate
            {
                TextEditor.TextArea.Text = text;
            });
                
            reader.Close();
        }

        private void UpdateFile()
        {
            if (openFileInfo == null) 
                return;
                    
            openFileInfo.Refresh();
            
            using var stream = new FileStream(openFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            using var writer = new StreamWriter(stream);
            stream.SetLength(0);
            writer.Write(TextEditor.TextArea.Text);
        }
    }
}