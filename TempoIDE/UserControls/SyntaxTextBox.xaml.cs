using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox : UserControl, IInputElement
    {
        public Rect CaretRect { get; private set; }
        public IntVector CaretOffset;
        public int CaretIndex { get; private set; }
        
        public IntRange SelectionRange = new IntRange(0, 0);

        public int TabSize = 4;
        public bool IsReadOnly;

        public event RoutedEventHandler TextChanged;

        private static readonly int CaretBlinkFrequencyMs = 500;

        private Thread caretThread;
        private bool overrideCaretVisibility;
        private bool caretVisible;

        private bool isSelecting;
        
        public AutoCompletionSelection AutoCompletions;

        public SyntaxTextBox()
        {
            InitializeComponent();
        }
        
        private void SyntaxTextBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            Focusable = true;
            IsTabStop = true;

            CaretRect = new Rect(0, 0, 1, TextArea.LineHeight);
        }

        #region Private Interface
        
        private void UpdateAutoCompletion()
        {
            var newCompletions = TextArea.Scheme?.GetAutoCompletions(this);

            if (newCompletions != null && newCompletions.Length != 0)
            {
                AutoCompletions = new AutoCompletionSelection(newCompletions);

                AutoComplete.Enable();
                AutoComplete.Update(AutoCompletions.Choices);

                AutoComplete.Translate.X = CaretRect.Right;
                AutoComplete.Translate.Y = CaretRect.Bottom;
                
                AutoComplete.SelectedIndex = 0;
            }
            else
            {
                AutoComplete.Visibility = Visibility.Collapsed;
            }
        }

        private int GetCaretIndexAtOffset(IntVector offset)
        {
            VerifyCaretOffset(offset, true);

            var totalIndex = 0;
            var lines = TextArea.GetLines();

            for (var lineNo = 0; lineNo < offset.Y; lineNo++)
            {
                totalIndex += lines[lineNo].Count;
            }
            
            totalIndex += offset.X;

            return totalIndex;
        }

        private IntVector GetCaretOffsetAtIndex(int index)
        {
            var lines = TextArea.GetLines();

            var indexCount = 0;
            var lineNo = 0;
            var columnNo = 0;

            foreach (var line in lines)
            {
                foreach (var column in line)
                {
                    if (indexCount == index)
                        return new IntVector(columnNo, lineNo);

                    columnNo++;
                    indexCount++;
                }

                lineNo++;
            }
            
            throw new Exception("Index must be valid.");
        }

        private bool VerifyCaretOffset(IntVector offset, bool throwError = false)
        {
            var lines = TextArea.GetLines();

            // ReSharper disable once ReplaceWithSingleAssignment.True
            var result = true;

            if (lines.Length <= offset.Y || offset.Y < 0)
                result = false;

            if (result && lines[offset.Y].Count < offset.X)
                result = false;

            if (!result && throwError)
                throw new Exception("Offset must be valid.");

            return result;
        }

        private void CaretBlinkerThread()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(CaretBlinkFrequencyMs);
                }
                catch (ThreadInterruptedException)
                {
                    return;
                }

                Dispatcher.Invoke(delegate { caretVisible = !caretVisible; });
                Dispatcher.Invoke(TextArea.InvalidateVisual);
            }
        }
        
        #endregion
    }
}