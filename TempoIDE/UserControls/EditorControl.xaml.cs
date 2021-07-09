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
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
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

        private CompletionWindow autoComplete;
        private string selectedAutoComplete;
        private string typingWord;

        public EditorControl()
        {
            InitializeComponent();
        }

        private void TextEditor_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextEditor.TextArea.TextEntering += TextArea_OnTextEntering;
            
            writerThread = new Thread(TextWriterThread);
            writerThread.Start();
        }

        private void TextArea_OnTextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && autoComplete != null) {
                if (!char.IsLetterOrDigit(e.Text[0])) {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    autoComplete.CompletionList.RequestInsertion(e);
                }
            }
        }

        private void TextEditor_OnTextChanged(object sender, EventArgs e)
        {
            if (skipTextChanged)
                return;
            
            textChangedBeforeUpdate = true;

            var suggestion = CsIntellisense.AutoCompleteSuggest(ref TextEditor);
            
            if (suggestion == null || suggestion.Count == 0)
            {
                autoComplete?.Close();
                return;
            }

            if (autoComplete is null)
            {
                autoComplete = new CompletionWindow(TextEditor.TextArea);
                autoComplete.Closed += (o, args) => autoComplete = null;
                
                var data = autoComplete.CompletionList.CompletionData;

                foreach (string word in suggestion)
                {
                    data.Add(new CompletionData(word + " "));
                }
                
                autoComplete.Show();
            }
        }

        private void SkipTextChange(Action method)
        {
            skipTextChanged = true;
            method.Invoke();
            skipTextChanged = false;
        }
    }
    
    public class CompletionData : ICompletionData
    {
        public CompletionData(string text)
        {
            this.Text = text;
        }
    
        public System.Windows.Media.ImageSource Image => null;

        public string Text { get; private set; }
    
        // Use this property if you want to show a fancy UIElement in the list.
        public object Content => Text;

        public object Description => "Description for " + Text;

        public double Priority => 1;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, Text);
        }
    }
}