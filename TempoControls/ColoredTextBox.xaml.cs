using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TempoControls.Core.Types;

namespace TempoControls
{
    public partial class ColoredTextBox
    {
        public Rect CaretRect { get; private set; }
        public IntVector CaretOffset;
        public int CaretIndex => GetIndexAtOffset(CaretOffset);
        public float CaretWidth { get; set; } = 1;

        public IntRange SelectionRange = new(0, 0);

        public int TabSize { get; set; } = 4;

        private bool isReadOnly;
        public bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                isReadOnly = value;
                Focusable = !value;

                if (!value && IsFocused)
                    Keyboard.ClearFocus();
            }
        }

        public event RoutedEventHandler TextChanged;

        private static readonly int CaretBlinkFrequencyMs = 500;

        private Thread caretThread;
        private bool overrideCaretVisibility;
        private bool caretVisible;

        private bool isSelecting;

        public ColoredTextBox()
        {
            InitializeComponent();
        }
        
        private void ColoredTextBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            CaretRect = new Rect(0, 0, CaretWidth, TextArea.LineHeight);
        }

        #region Private Interface
        
        private void UpdateAutoCompletion()
        {
            var newCompletions = TextArea.CompletionProvider?.GetAutoCompletions(this);

            if (newCompletions != null && newCompletions.Length != 0)
            {
                AutoComplete.Enable();
                AutoComplete.Update(new AutoCompletionSelection(newCompletions));

                AutoComplete.Translate.X = CaretRect.Right;
                AutoComplete.Translate.Y = CaretRect.Bottom;
                
                AutoComplete.SelectedIndex = 0;
            }
            else
            {
                AutoComplete.Visibility = Visibility.Collapsed;
            }
        }

        private int GetIndexAtOffset(IntVector offset)
        {
            if (!VerifyOffset(offset))
                return -1;
            
            var totalIndex = 0;
            var lines = TextArea.GetLines();

            for (var lineNo = 0; lineNo < offset.Y; lineNo++)
            {
                totalIndex += lines[lineNo].Count;
            }
            
            totalIndex += offset.X;

            return totalIndex;
        }

        private IntVector GetOffsetAtIndex(int index)
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
            
            if (indexCount == index)
                return new IntVector(columnNo, lineNo);
            
            throw new Exception("Index must be valid.");
        }

        private bool VerifyIndex(int index)
        {
            return index >= 0 && index < TextArea.Characters.Count;
        }

        private bool VerifyOffset(IntVector offset)
        {
            if (offset.X < 0 || offset.Y < 0)
                return false;
            
            var lines = TextArea.GetLines();

            if (lines.Length <= offset.Y || offset.Y < 0)
            {
                return false;
            }
            
            if (lines[offset.Y].Count < offset.X)
            {
                return false;
            }
            
            return true;
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