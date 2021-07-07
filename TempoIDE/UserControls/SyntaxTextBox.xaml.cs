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

        public Rect CaretPosition { get; private set; }
        public Vector CaretOffset { get; private set; }
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

            CaretPosition = new Rect(0, 0, 2, LineHeight);

            TextChanged += SyntaxTextBox_OnTextChanged;
        }
        
        #region Public Interface

        public void AppendText(string text)
        {
            foreach (var character in text)
            {
                characters.Add((SyntaxChar)character);
                CaretIndex++;

                if (character is NewLine)
                {
                    CaretOffset = new Vector(0, CaretOffset.Y + 1);
                }
                else
                {
                    CaretOffset = new Vector(CaretOffset.X + 1, CaretOffset.Y);
                }
            }
            
            TextChanged?.Invoke(this, default);
        }
        
        public void AppendText(char character)
        {
            AppendText(character.ToString());
        }
        
        public void AppendText(SyntaxChar character)
        {
            AppendText(character.ToString());
        }

        [Obsolete("This method overload has a O(n^2) time complexity, it is recommended that you pass a string instead.")]
        public void AppendText(IEnumerable<char> syntaxChars)
        {
            foreach (var character in syntaxChars)
                AppendText(character.ToString());
        }
        
        public void AppendText(IEnumerable<SyntaxChar> syntaxChars)
        {
            foreach (var character in syntaxChars)
                AppendText(character.ToString());
        }

        public void Backspace(int count)
        {
            if (characters.Count == 0)
                return;
            
            for (; count > 0; count--)
            {
                SyntaxChar character = characters[^1];
                
                characters.RemoveAt(CaretIndex - 1);
                CaretIndex--;

                if (character.ToChar() is NewLine)
                {
                    CaretOffset = new Vector(0, CaretOffset.Y + 1);
                }
                else
                {
                    CaretOffset = new Vector(CaretOffset.X - 1, CaretOffset.Y);
                }
            }

            TextChanged?.Invoke(this, default);
        }

        public void Clear()
        {
            characters.Clear();

            CaretIndex = 0;
            CaretOffset = new Vector();

            TextChanged?.Invoke(this, default);
        }
        
        public void SetScheme(string schemeExtension)
        {
            scheme = ColorScheme.GetColorSchemeByExtension(schemeExtension);
        }
        
        public IEnumerator<SyntaxChar> EnumerateCharacters()
        {
            foreach (var character in characters)
                yield return character;
        }

        public void Highlight()
        {
            var stb = this;
            scheme?.Highlight(ref stb);
        }
        
        #endregion

        #region Private Interface
        
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

                Dispatcher.Invoke(delegate { caretVisible = true; });
                Dispatcher.Invoke(InvalidateVisual);
            }
        }
        
        #endregion
    }
}