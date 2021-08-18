using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;
using TempoIDE.Core.Static;

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
            TextEditor.TextArea.AfterHighlight += TextEditor_OnAfterHighlight;
            
            writerThread = new Thread(TextWriterThread);
            writerThread.Start();
        }

        private void TextEditor_OnTextChanged(object sender, RoutedEventArgs e)
        {
            textChangedBeforeUpdate = true;
        }

        private void TextEditor_OnAfterHighlight(SyntaxCharCollection syntaxCharCollection)
        {
            // TODO: THIS HOLY SHIT FINALLY
        }

        public override bool TryCopy()
        {
            var text = TextEditor.GetSelectedText().ToString();
            
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
            
            if (TextEditor.GetSelectedText().Count == 0)
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
            var text = TextEditor.GetSelectedText().ToString();

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

        public override void Refresh() => UpdateText();

        public override void Update(FileInfo file)
        {
            if (!file?.Exists ?? false)
                return;

            BoundFile = file;

            UpdateText();

            TextEditor.IsReadOnly = file == null;

            TextEditor.TextArea.SetScheme(
                ColoredLabelAssociator.SchemeFromExtension(BoundFile?.Extension));
            
            TextEditor.TextArea.SetCompletionProvider(
                ColoredLabelAssociator.CompletionProviderFromExtension(BoundFile?.Extension));
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

            await using var stream = BoundFile.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            stream.Seek(0, SeekOrigin.End);

            var text = TextEditor.TextArea.Text;
           
            await stream.WriteAsync(ApplicationHelper.GlobalEncoding.GetBytes(text), 0, text.Length);
        }

        private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextEditor.Focus();
        }
    }
}