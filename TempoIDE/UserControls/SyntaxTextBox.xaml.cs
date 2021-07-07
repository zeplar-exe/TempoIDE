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

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox : UserControl
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
                    AppendText(value);
                }
            }

        public Rect CaretPosition { get; private set; } // TODO: Update this whenever needed
        
        private IntVector caretOffset;
        public IntVector CaretOffset
        {
            get => caretOffset;
            private set
            {
                if (!VerifyCaretOffset(value))
                    throw new Exception("Offset must be valid.");

                caretOffset = value;
                CaretIndex = GetCaretIndexAtOffset(value);

                var lines = GetLines();

                CaretPosition = new Rect();

                for (int columnNo = 0; columnNo < value.X; columnNo++)
                {
                    CaretPosition = Rect.Offset(CaretPosition, characters[columnNo].Width, 0);
                }
                
                for (int lineNo = 0; lineNo < value.Y; lineNo++)
                {
                    CaretPosition = Rect.Offset(CaretPosition, 0, LineHeight);
                }
            }
        }
        
        public int CaretIndex { get; private set; }

        public bool IsReadOnly;

        public int CharacterLeftRightMargin = 2;
        public int LineHeight = 15;
        public new int FontSize = 14;

        public event RoutedEventHandler TextChanged;

        private static readonly int CaretBlinkFrequencyMs = 500;
        private const char NewLine = '\n';
        
        private Thread caretThread;
        private bool caretVisible;

        private IColorScheme scheme;
        private readonly List<SyntaxChar> characters = new List<SyntaxChar>();

        public SyntaxTextBox()
        {
            InitializeComponent();
        }
        
        private void SyntaxTextBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            Focusable = true;
            IsTabStop = true;

            CaretPosition = new Rect(0, 0, 0.2, LineHeight);

            TextChanged += SyntaxTextBox_OnTextChanged;
        }

        #region Private Interface

        private int GetCaretIndexAtOffset(IntVector offset)
        {
            if (!VerifyCaretOffset(offset))
                throw new Exception("Offset must be valid.");

            var totalIndex = 0;
            var lines = GetLines();

            for (int lineNo = 0; lineNo < offset.Y; lineNo++)
            {
                totalIndex += lines[lineNo].Length - 1;
            }

            totalIndex += offset.X;

            return totalIndex;
        }

        private bool VerifyCaretOffset(IntVector offset)
        {
            var lines = GetLines();

            if (lines.Length <= offset.Y || offset.Y < 0)
                return false;

            if (lines[offset.Y].Length < offset.X)
                return false;

            return true;
        }

        private string[] GetLines()
        {
            return Text.Split(NewLine);
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