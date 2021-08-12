using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Classes;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class FileEditor : Editor
    {
        private Thread writerThread;
        
        private bool textChangedBeforeUpdate;

        private Encoding encoding;
        
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
            
            UpdateCullingRange();
        }
        
        private void ScrollView_OnScrollChanged(object sender, RoutedEventArgs e)
        {
            UpdateCullingRange();
        }

        private void UpdateCullingRange()
        {
            TextEditor.TextArea.CullingRange = new Rect(
                0,
                0,
                ScrollView.HorizontalOffset.Safe() + TextEditor.ActualWidth.Safe(),
                ScrollView.VerticalOffset.Safe() + TextEditor.ActualHeight.Safe()
            );
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
            
            Console.WriteLine(text);
            
            Clipboard.SetText(text, TextDataFormat.Text);
                
            return true;

        }

        public override bool TryPaste()
        {
            var text = Clipboard.GetText();

            if (string.IsNullOrEmpty(text))
                return false;
            
            if (TextEditor.GetSelectedText() == string.Empty)
            {
                TextEditor.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.Text));   
            }
            else
            {
                TextEditor.Backspace(0);
                TextEditor.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.Text));
            }
            
            return true;
        }

        public override bool TryCut()
        {
            var text = TextEditor.GetSelectedText();

            if (string.IsNullOrEmpty(text)) 
                return false;
            
            Clipboard.SetText(text, TextDataFormat.Text);
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
            if (!file?.Exists ?? false)
                return;

            BoundFile = file;

            var cached = EnvironmentHelper.Cache.GetFile(BoundFile);

            encoding = cached?.Encoding ?? Encoding.ASCII;

            UpdateText();

            TextEditor.IsReadOnly = file == null;
            TextEditor.TextArea.SetScheme(BoundFile?.Extension);
        }

        private void TextWriterThread()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(WriterCooldown * 1000);
                }
                catch (ThreadInterruptedException _) { }
                finally
                {
                    Dispatcher.Invoke(TextWriter);;
                }
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
            textChangedBeforeUpdate = false;
        }

        public override async void UpdateFile()
        {
            if (BoundFile == null) 
                return;
                    
            BoundFile.Refresh();
            
            using var stream = BoundFile.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            stream.Seek(0, SeekOrigin.End);

            var text = TextEditor.TextArea.Text;
            
            await stream.WriteAsync(encoding.GetBytes(text), 0, text.Length);
        }

        private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextEditor.Focus();
        }
    }
}