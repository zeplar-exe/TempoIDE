using System;
using System.IO;
using System.Threading;
using System.Windows;
using TempoIDE.Core.Associators;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Controls.Editors
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

        public static TextFileEditor FromFile(FileInfo file)
        {
            var editor = file.Extension.Replace(".", "") switch
            {
                "cs" => new CsFileEditor(),
                _ => new TextFileEditor()
            };
            
            editor.Update(file);

            return editor;
        }

        private void TextEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            Repeat.Interval(TimeSpan.FromMilliseconds(WriterCooldown), FileWriter, CancellationToken.None);
        }
        
        public override void FileWriter()
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
            
            var file = EnvironmentHelper.Current.Cache.GetFile(BoundFile);

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
            
            File.WriteAllText(BoundFile.FullName, string.Concat(TextBox.TextArea.GetLines()),
                ApplicationHelper.GlobalEncoding);
        }

        private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox.Focus();
        }
    }
}