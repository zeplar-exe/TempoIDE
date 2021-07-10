using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Classes;
using TempoIDE.Classes.ColorSchemes;
using TempoIDE.Classes.EditorCommands;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox : UserControl, IInputElement
    {
        public string Text
        {
            get
            {
                var stringBuilder = new StringBuilder();
                
                foreach (var character in characters)
                {
                    stringBuilder.Append(character.Value);
                }

                return stringBuilder.ToString();
            }
            set
                {
                    Clear();
                    AppendTextAtCaret(value);
                }
            }

        public Rect CaretRect { get; private set; }
        
        private IntVector caretOffset;
        public IntVector CaretOffset
        {
            get => caretOffset;
            internal set
            {
                if (!VerifyCaretOffset(value))
                    return;

                caretOffset = value;
                CaretIndex = GetCaretIndexAtOffset(value);
                
                CaretRect = new Rect(0, 0, CaretRect.Width, CaretRect.Height);

                var line = GetLines()[value.Y];
                
                for (int columnNo = 0; columnNo < value.X; columnNo++)
                {
                    CaretRect = Rect.Offset(CaretRect, line[columnNo].Size.Width, 0);
                }
                
                CaretRect = Rect.Offset(CaretRect, 0, LineHeight * value.Y);
            }
        }

        public int CaretIndex { get; private set; }

        public IntRange SelectionRange = new IntRange(0, 0);
        public bool IsReadOnly = true;
        
        public int LineHeight = 15;
        public new int FontSize = 14;

        public readonly List<IEditorCommand> Commands = new List<IEditorCommand>()
        {
            new Copy(),
            new Paste(),
            new Cut(),
            new SelectAll()
        };

        public event RoutedEventHandler TextChanged;

        private static readonly int CaretBlinkFrequencyMs = 500;
        private const char NewLine = '\r';
        
        private Thread caretThread;
        private bool caretVisible;

        private ISyntaxScheme scheme;
        private readonly List<SyntaxChar> characters = new List<SyntaxChar>();
        private bool isSelecting;

        private string selectedAutoComplete;

        public SyntaxTextBox()
        {
            InitializeComponent();
        }
        
        private void SyntaxTextBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            Focusable = true;
            IsTabStop = true;

            CaretRect = new Rect(0, 0, 1, LineHeight);

            TextChanged += SyntaxTextBox_OnTextChanged;
        }

        #region Private Interface

        private int GetCaretIndexAtOffset(IntVector offset)
        {
            VerifyCaretOffset(offset, true);

            var totalIndex = 0;
            var lines = GetLines();

            for (int lineNo = 0; lineNo < offset.Y; lineNo++)
            {
                totalIndex += lines[lineNo].Count;
            }
            
            totalIndex += offset.X;

            return totalIndex;
        }

        private IntVector GetCaretOffsetAtIndex(int index)
        {
            var lines = GetLines();

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
            var lines = GetLines();

            // ReSharper disable once ReplaceWithSingleAssignment.True
            bool result = true;

            if (lines.Length <= offset.Y || offset.Y < 0)
                result = false;

            if (result && lines[offset.Y].Count < offset.X)
                result = false;

            if (!result && throwError)
                throw new Exception("Offset must be valid.");

            return result;
        }
        
        private int GetLineCount()
        {
            return GetLines().Length;
        }

        private List<SyntaxChar>[] GetLines(bool omitNewLines = false)
        {
            List<List<SyntaxChar>> lines = new List<List<SyntaxChar>> { new List<SyntaxChar>() };
            int currentIndex = 0;

            foreach (var character in characters)
            {
                if (character.Value == NewLine)
                {
                    if (!omitNewLines)
                        lines[currentIndex].Add(character);
                    
                    currentIndex++;

                    lines.Add(new List<SyntaxChar>());
                }
                else
                {
                    lines[currentIndex].Add(character);
                }
            }
            
            return lines.ToArray();
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
                Dispatcher.Invoke(InvalidateVisual);
            }
        }
        
        #endregion
    }
}