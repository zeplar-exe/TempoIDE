using System;
using System.IO;
using System.Threading;
using System.Windows;
using TempoIDE.Classes;
using Microsoft.Build;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class FileEditor : Editor
    {
        private Thread writerThread;
        
        private bool textChangedBeforeUpdate;
        
        private const int WriterCooldown = 2;

        public override bool IsFocused => TextEditor.IsFocused;

        public FileEditor()
        {
            InitializeComponent();
        }
        
        private void TextEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            writerThread = new Thread(TextWriterThread);
            writerThread.Start();
        }

        private void TextEditor_OnTextChanged(object sender, RoutedEventArgs e)
        {
            textChangedBeforeUpdate = true;
        }
        
        public override bool TryCopy()
        {
            var text = TextEditor.GetSelectedText();
            
            if (string.IsNullOrEmpty(text)) 
                return false;
            
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
                
            return true;

        }

        public override bool TryPaste()
        {
            var text = Clipboard.GetText();

            if (string.IsNullOrEmpty(text))
                return false;
            
            if (TextEditor.GetSelectedText() == string.Empty)
            {
                TextEditor.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));   
            }
            else
            {
                TextEditor.Backspace(0);
                TextEditor.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));
            }
            
            return true;
        }

        public override bool TryCut()
        {
            var text = TextEditor.GetSelectedText();
            
            if (string.IsNullOrEmpty(text)) 
                return false;
            
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
            TextEditor.Backspace(0);
        
            return true;
        }
        
        public override bool TrySelectAll()
        {
            TextEditor.Select(new IntRange(0, TextEditor.TextArea.Text.Length));
            
            return true;
        }

        public override void Refresh()
        {
            TextEditor.Clear();

            var text = BoundFile == null
                ? string.Empty
                : new StreamReader(BoundFile.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).ReadToEnd();
            
            TextEditor.AppendTextAtCaret(text);
            TextEditor.IsReadOnly = BoundFile == null;
        }

        public override void Update(FileInfo file)
        {
            if (file != null && !file.Exists)
                return;

            BoundFile = file;
            TextEditor.TextArea.SetScheme(BoundFile?.Extension);
            
            UpdateText();

            TextEditor.IsReadOnly = file == null;
            
            TextEditor.TextArea.Scheme?.Highlight(TextEditor.TextArea);
        }

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
        
        public override void TextWriter()
        {
            if (textChangedBeforeUpdate)
            {
                UpdateFile();
            }
            else
            {
                UpdateText();
            }

            textChangedBeforeUpdate = false;
        }

        public override void UpdateText()
        {
            if (BoundFile == null) 
                return;
                    
            BoundFile.Refresh();
            
            var text = EnvironmentHelper.Cache.GetFile(BoundFile);

            if (text == null || text.Content == TextEditor.TextArea.Text)
                return;

            TextEditor.TextArea.Text = text.Content;
        }

        public override void UpdateFile()
        {
            if (BoundFile == null) 
                return;
                    
            BoundFile.Refresh();

            using var stream = new FileStream(BoundFile.FullName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            using var writer = new StreamWriter(stream);
            stream.SetLength(0);
            
            writer.WriteAsync(TextEditor.TextArea.Text);
        }

        private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextEditor.Focus();
        }
    }
}