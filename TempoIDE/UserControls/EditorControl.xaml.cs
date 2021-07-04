using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using TempoIDE.Classes;

namespace TempoIDE.UserControls
{
    public partial class EditorControl : Grid
    {
        private Thread writerThread;

        private FileInfo openFileInfo;
        private readonly OrderedDictionary<FileInfo, EditorTabButton> openFiles = new OrderedDictionary<FileInfo, EditorTabButton>();

        private bool textChangedBeforeUpdate;
        private bool skipTextChanged;

        private string selectedAutoComplete;
        private string typingWord;

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
            
            string text = file != null ?
                new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd() : string.Empty;
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
        }
        
        private void TextEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            writerThread = new Thread(TextWriterThread);
            writerThread.Start();
        }
        
        private void ExplorerPanel_OnOpenFileEvent(object sender, OpenFileEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                OpenFile(e.NewFile);
            });
        }

        private void TextEditor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (skipTextChanged)
                return;
            
            textChangedBeforeUpdate = true;

            SkipTextChange(delegate
            {
                CsIntellisense.Highlight(ref TextEditor);
            });
            
            var suggestion = CsIntellisense.Suggest(ref TextEditor);

            if (suggestion is null)
                return;
            
            typingWord = suggestion.Item1;
            var completeWords = suggestion.Item2;

            if (completeWords == null || completeWords.Count == 0)
            {
                selectedAutoComplete = null;
                AutoComplete.Visibility = Visibility.Collapsed;
                return;
            }
            
            AutoComplete.Visibility = Visibility.Visible;

            var caretPosition = TextEditor.CaretPosition.GetCharacterRect(LogicalDirection.Forward);

            AutoComplete.Translate.X = caretPosition.Right;
            AutoComplete.Translate.Y = caretPosition.Bottom;

            selectedAutoComplete = completeWords[0];

            AutoComplete.Words.Children.Clear();

            foreach (string word in completeWords)
            {
                AutoComplete.Words.Children.Add(new TextBlock { Text = word });
            }
        }

        private void TextEditor_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                TextEditor.AppendText(selectedAutoComplete.Replace(typingWord ??= "", ""));
                AutoComplete.Visibility = Visibility.Collapsed;
            }
        }

        private void SkipTextChange(Action method)
        {
            skipTextChanged = true;
            method.Invoke();
            skipTextChanged = false;
        }
        
        private const int WriterCooldown = 5;
        private void TextWriterThread()
        {
            while (true) // TODO: Figure out how to use IsLoaded here
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
                var reader = new StreamReader(new FileStream(openFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                var text = reader.ReadToEnd();

                if (text == TextEditor.GetPlainText())
                    return;
                    
                SkipTextChange(delegate
                {
                    TextEditor.Document.Blocks.Clear();
                    TextEditor.AppendText(reader.ReadToEnd()); 
                });
                
                reader.Close();
            }
            else
            {
                using var stream = new FileStream(openFileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                using var writer = new StreamWriter(stream);
                stream.SetLength(0);
                writer.Write(TextEditor.GetPlainText());
            }

            textChangedBeforeUpdate = false;
        }
    }
}