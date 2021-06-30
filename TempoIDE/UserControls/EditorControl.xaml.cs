using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Classes;

namespace TempoIDE.UserControls
{
    public partial class EditorControl : Grid
    {
        private Thread writerThread;

        private FileInfo openFileInfo;
        private readonly OrderedDictionary<FileInfo, EditorTabButton> openFiles = new OrderedDictionary<FileInfo, EditorTabButton>();

        private bool textChangedBeforeUpdate;
        
        public EditorControl()
        {
            InitializeComponent();
        }

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
            
            string text = file != null ?
                new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd() : string.Empty;
            TextEditor.AppendText(text);
            TextEditor.IsReadOnly = file == null;
            CurrentFileNameDisplay.Text = file?.FullName;
        }

        public void CloseFile(FileInfo file)
        {
            openFiles.Remove(file);
            ReloadOpenFiles();
        }

        private void FileButton_OnClick(object sender, FileTabEventArgs e)
        {
            CsIntellisense.Highlight(ref TextEditor);
            
            TextWriter();
            OpenFile((FileInfo)e.TabButton.Resources["FileInfo"]);
        }

        private void FileClose_OnClick(object sender, FileTabEventArgs e)
        {
            int index = openFiles.IndexOf((FileInfo)e.TabButton.Resources["FileInfo"]);
            int nextIndex = index;
            int lastIndex = index - 1;

            TextWriter();
            CloseFile((FileInfo)e.TabButton.Resources["FileInfo"]);
            
            if (index == 0)
            {
                if (nextIndex < openFiles.Count)
                    OpenFile((FileInfo)openFiles[nextIndex].Value.Resources["FileInfo"]);
            }
            else
            {
                OpenFile((FileInfo)openFiles[lastIndex].Value.Resources["FileInfo"]);
            }
            // TODO: This
        }
        
        private void TextEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            writerThread = new Thread(TextWriterThread);
            writerThread.Start();
        }
        
        private void TextEditor_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            int line = 0; //TextEditor.GetLineIndexFromCharacterIndex(TextEditor.SelectionStart);
            int column = 0; //TextEditor.SelectionStart - TextEditor.GetCharacterIndexFromLineIndex(line);

            LineIndexDisplay.Text = $"Caret Position: Line {line + 1} at Position {column + 1}";
        }

        private void TextEditor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            textChangedBeforeUpdate = true;
            CsIntellisense.Highlight(ref TextEditor);
        }
        
        private const int WriterCooldown = 5;
        private void TextWriterThread()
        {
            while (true)
            {
                Thread.Sleep(WriterCooldown * 1000);
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
                TextEditor.Document.Blocks.Clear();

                using var reader = new StreamReader(new FileStream(openFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                TextEditor.AppendText(reader.ReadToEnd());
            }
            else
            {
                using var writer = new StreamWriter(new FileStream(openFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite));
                writer.Write(TextEditor.GetPlainText());
            }

            textChangedBeforeUpdate = false;
        }
    }

    public delegate void FileTabEventHandler(object sender, FileTabEventArgs e);

    public class FileTabEventArgs : RoutedEventArgs
    {
        public EditorTabButton TabButton;

        public FileTabEventArgs(EditorTabButton tabButton)
        {
            TabButton = tabButton;
        }
    }
}