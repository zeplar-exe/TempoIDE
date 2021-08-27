using System;
using System.IO;
using System.Threading;
using System.Windows;
using TempoControls.Core.Static;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;
using TempoIDE.Core.Static;

namespace TempoIDE.UserControls
{
    public partial class TextEditor : Editor
    {
        private Thread writerThread;
        
        private bool textChangedBeforeUpdate;

        private const int WriterCooldown = 2;

        public override bool IsFocused => TextBox.IsFocused;

        public TextEditor()
        {
            InitializeComponent();
            
            TextBox.TextArea.AfterHighlight += TextEditor_OnAfterHighlight;
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

        private void TextEditor_OnAfterHighlight(SyntaxCharCollection charCollection)
        {
            var inspector = ExtensionAssociator.InspectorFromExtension(BoundFile?.Extension);
            var project = EnvironmentHelper.GetProjectOfFile(BoundFile);

            inspector.Inspect(charCollection, project?.Compilation);
        }

        public override bool TryCopy()
        {
            var text = TextBox.GetSelectedText();
            
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
            
            if (TextBox.GetSelectedText().Length == 0)
            {
                TextBox.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));   
            }
            else
            {
                TextBox.Backspace(0);
                TextBox.AppendTextAtCaret(Clipboard.GetText(TextDataFormat.UnicodeText));
            }
            
            return true;
        }

        public override bool TryCut()
        {
            var text = TextBox.GetSelectedText();

            if (string.IsNullOrEmpty(text)) 
                return false;
            
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
            TextBox.Backspace(0);
        
            return true;
        }
        
        public override bool TrySelectAll()
        {
            TextBox.Select(new IntRange(0, TextBox.TextArea.TextBuilder.Length));
            
            return true;
        }

        public override void Refresh() => UpdateText();

        public override void Update(FileInfo file)
        {
            if (!file?.Exists ?? false)
                return;

            BoundFile = file;

            UpdateText();

            TextBox.IsReadOnly = file == null;

            TextBox.TextArea.SetScheme(
                ExtensionAssociator.SchemeFromExtension(BoundFile?.Extension));
            
            TextBox.TextArea.SetCompletionProvider(
                ExtensionAssociator.CompletionProviderFromExtension(BoundFile?.Extension));
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

            if (text == null || text.Content == TextBox.TextArea.Text)
                return;

            TextBox.TextArea.TextBuilder.SetString(text.Content);
            textChangedBeforeUpdate = false;
        }

        public override async void UpdateFile()
        {
            if (BoundFile == null) 
                return;
                    
            BoundFile.Refresh();

            await using var stream = BoundFile.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            stream.Seek(0, SeekOrigin.End);

            var text = TextBox.TextArea.Text;
            
            foreach (var line in TextBox.TextArea.GetLines())
            {
                await stream.WriteAsync(
                    ApplicationHelper.GlobalEncoding.GetBytes(text + Environment.NewLine), 
                    0, line.Length);
            }
        }

        private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox.Focus();
        }
    }
}