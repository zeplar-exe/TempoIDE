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

        private void TextEditor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (skipTextChanged)
                return;
            
            textChangedBeforeUpdate = true;

            SkipTextChange(delegate
            {
                TextEditor.Highlight();
            });
            
            var suggestion = CsIntellisense.AutoCompleteSuggest(ref TextEditor);

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

            var caretPosition = TextEditor.CaretRect;

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
            if (e.Key == Key.Tab && selectedAutoComplete != null)
            {
                string newText = selectedAutoComplete.Remove(0, typingWord.Length);

                TextEditor.AppendText(newText);
                AutoComplete.Visibility = Visibility.Collapsed;
                
                TextEditor.Focus();
            }
        }

        private void SkipTextChange(Action method)
        {
            skipTextChanged = true;
            method.Invoke();
            skipTextChanged = false;
        }
    }
}