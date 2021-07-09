using System;
using System.IO;
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
        private bool skipTextChanged;

        private string selectedAutoComplete;
        private string typingWord;

        public EditorControl()
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
            if (skipTextChanged)
                return;
            
            textChangedBeforeUpdate = true;
        }

        private void SkipTextChange(Action method)
        {
            skipTextChanged = true;
            method.Invoke();
            skipTextChanged = false;
        }
    }
}