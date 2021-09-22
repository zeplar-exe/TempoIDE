using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using TempoControls;
using TempoControls.Core.IntTypes;
using TempoIDE.Core.Static;

namespace TempoIDE.UserControls.Editors
{
    public partial class TextFileEditor : FileEditor
    {
        private const int WriterCooldown = 500;
        private bool textChangedBeforeUpdate;

        public override bool IsFocused => TextBox.IsFocused;

        public TextFileEditor()
        {
            InitializeComponent();
        }

        public static TextFileEditor FromExtension(string extension)
        {
            return extension.Replace(".", "") switch
            {
                "cs" => new CsFileEditor(),
                _ => new TextFileEditor()
            };
        }

        private void TextEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            Repeat.Interval(TimeSpan.FromMilliseconds(WriterCooldown), TextWriter, CancellationToken.None);
        }
        
        public override void TextWriter()
        {
            if (textChangedBeforeUpdate)
            {
                UpdateFile();
            }
            else
            {
                Dispatcher.Invoke(UpdateVisual);
            }

            textChangedBeforeUpdate = false;
        }

        private void TextEditor_OnTextChanged(object sender, RoutedEventArgs e)
        {
            textChangedBeforeUpdate = true;
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

        public override void UpdateVisual()
        {
            if (BoundFile == null) 
                return;
                    
            BoundFile.Refresh();
            
            var file = EnvironmentHelper.Cache.GetFile(BoundFile);

            if (file == null || file.Content == TextBox.TextArea.Text)
                return;

            TextBox.Clear();
            TextBox.AppendTextAtCaret(file.Content);
            
            textChangedBeforeUpdate = false;
        }

        public override void UpdateFile()
        {
            if (BoundFile == null)
                return;

            BoundFile.Refresh();
            
            File.WriteAllText(BoundFile.FullName, string.Concat(TextBox.TextArea.GetLines()));

            // using var stream = BoundFile.OpenRead();
            // using var writer = new StreamWriter(stream, ApplicationHelper.GlobalEncoding) { AutoFlush = false };
            //
            // foreach (var line in TextBox.TextArea.GetLines())
            //     writer.Write(line);
            //
            // writer.Flush();
        }

        private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox.Focus();
        }
    }
}