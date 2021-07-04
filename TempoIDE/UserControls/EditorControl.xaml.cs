using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
            TextEditor.AcceptsTab = true;
            
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
                CsIntellisense.Highlight(ref TextEditor);
            });
            
            var suggestion = CsIntellisense.AutoCompleteSuggest(ref TextEditor);

            if (suggestion is null)
                return;

            TextEditor.AcceptsTab = false;
            
            typingWord = suggestion.Item1;
            var completeWords = suggestion.Item2;

            if (completeWords == null || completeWords.Count == 0)
            {
                selectedAutoComplete = null;
                AutoComplete.Visibility = Visibility.Collapsed;
                return;
            }
            
            AutoComplete.Visibility = Visibility.Visible;

            var caretPosition = TextEditor.CaretPosition.GetCharacterRect(LogicalDirection.Forward);

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

                var startPoint = TextEditor.Document.ContentStart;
                var caretOffset = startPoint.GetOffsetToPosition(TextEditor.CaretPosition);
                TextEditor.CaretPosition = startPoint.GetPositionAtOffset(caretOffset + newText.Length);

                TextEditor.AcceptsTab = true;
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