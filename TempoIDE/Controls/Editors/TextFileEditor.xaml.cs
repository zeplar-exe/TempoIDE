using System;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows;
using TempoIDE.Controls.CodeEditing;
using TempoIDE.Core.Associators;
using TempoIDE.Core.Helpers;
using Timer = System.Timers.Timer;

namespace TempoIDE.Controls.Editors
{
    public partial class TextFileEditor : FileEditor
    {
        private readonly Timer timer = new(500);
        private bool textChangedBeforeUpdate;

        public override bool IsFocused => Codespace.IsFocused;

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
            timer.Elapsed += delegate { FileWriter(); };
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

        private void TextEditor_OnTextChanged(Codespace codespace)
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

            Codespace.IsReadonly = file == null;

            // Codespace.TextArea.SetScheme(
            //     ExtensionAssociator.SchemeFromExtension(BoundFile?.Extension));
            //
            // Codespace.TextArea.SetCompletionProvider(
            //     ExtensionAssociator.CompletionProviderFromExtension(BoundFile?.Extension));
        }

        public override void UpdateVisual()
        {
            if (BoundFile == null) 
                return;
                    
            BoundFile.Refresh();
            
            var file = EnvironmentHelper.Current.Cache.GetFile(BoundFile);

            if (file == null || file.Content == Codespace.Text)
                return;

            Codespace.Clear();
            Codespace.Insert(0, file.Content);
            
            textChangedBeforeUpdate = false;
        }

        public override void UpdateFile()
        {
            if (BoundFile == null)
                return;

            BoundFile.Refresh();
            
            EnvironmentHelper.Current.DirectoryWatcher.Buffer();
            
            File.WriteAllText(BoundFile.FullName, Codespace.Text,
                ApplicationHelper.GlobalEncoding);
            
            EnvironmentHelper.Current.DirectoryWatcher.Resume();
        }

        private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Codespace.Focus();
        }
    }
}