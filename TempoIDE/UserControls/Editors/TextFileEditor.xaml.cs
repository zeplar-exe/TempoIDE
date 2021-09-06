using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using TempoControls.Core.IntTypes;
using TempoControls.Core.Static;
using TempoControls.Core.Types.Collections;
using TempoIDE.Core.Static;

namespace TempoIDE.UserControls.Editors
{
    public partial class TextFileEditor : FileEditor
    {
        private Thread writerThread;
        
        private bool textChangedBeforeUpdate;

        private const int WriterCooldown = 500;

        public override bool IsFocused => TextBox.IsFocused;

        public TextFileEditor()
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

        public override void Refresh() => UpdateVisual();

        public override void Update(FileInfo file)
        {
            if (!file?.Exists ?? false)
                return;
            
            BoundFile = file;

            UpdateVisual();

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
                    Thread.Sleep(WriterCooldown);
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
                UpdateVisual();
            }

            textChangedBeforeUpdate = false;
        }

        public override void UpdateVisual()
        {
            if (BoundFile == null) 
                return;
                    
            BoundFile.Refresh();
            
            var file = EnvironmentHelper.Cache.GetFile(BoundFile);

            if (file == null || file.Content == TextBox.TextArea.Text)
                return;

            TextBox.TextArea.TextBuilder.SetString(file.Content);
            textChangedBeforeUpdate = false;
        }

        public override void UpdateFile()
        {
            if (BoundFile == null) 
                return;
                    
            BoundFile.Refresh();

            using var stream = BoundFile.Open(FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using var writer = new StreamWriter(stream, ApplicationHelper.GlobalEncoding);
            stream.Seek(0, SeekOrigin.End);

            var lines = TextBox.TextArea.Text.Split(Environment.NewLine);
            var last = lines.Last();
            
            foreach (var line in lines)
            {
                writer.WriteAsync(line);

                if (line != last && line.Length > 1)
                    writer.WriteAsync(Environment.NewLine);
            }
        }

        private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox.Focus();
        }
    }
}