using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Classes;
using TempoIDE.Classes.ColorSchemes;

namespace TempoIDE.UserControls
{
    public partial class ColoredLabel : UserControl
    {
        public string Text
        {
            get
            {
                var stringBuilder = new StringBuilder();
                
                foreach (var character in Characters)
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
        
        
        public int LineHeight = 15;
        public new int FontSize = 14;

        public event RoutedEventHandler TextChanged;

        public const char NewLine = '\n';

        internal ISyntaxScheme Scheme { get; private set; }
        internal readonly List<SyntaxChar> Characters = new List<SyntaxChar>();

        public ColoredLabel()
        {
            InitializeComponent();
        }
        
        #region Events
        
        public delegate void BeforeRender(DrawingContext context);
        public delegate void BeforeCharacterRender(DrawingContext context, Rect rect, int index);
        public delegate void AfterCharacterRender(DrawingContext context, Rect rect, int index);
        public delegate void AfterRender(DrawingContext context);
        
        public BeforeRender OnBeforeRender
        {
            get => (BeforeRender)GetValue(OnBeforeRenderProperty);
            set => SetValue(OnBeforeRenderProperty, value);
        }

        public static readonly DependencyProperty OnBeforeRenderProperty =
            DependencyProperty.Register(nameof(OnBeforeRender), typeof(BeforeRender), typeof(ColoredLabel));

        
        public BeforeCharacterRender OnBeforeCharacterRender
        {
            get => (BeforeCharacterRender)GetValue(OnBeforeCharacterRenderProperty);
            set => SetValue(OnBeforeCharacterRenderProperty, value);
        }

        public static readonly DependencyProperty OnBeforeCharacterRenderProperty =
            DependencyProperty.Register(nameof(OnBeforeCharacterRender), typeof(BeforeCharacterRender), typeof(ColoredLabel));

        public AfterCharacterRender OnAfterCharacterRender
        {
            get => (AfterCharacterRender)GetValue(OnAfterCharacterRenderProperty);
            set => SetValue(OnAfterCharacterRenderProperty, value);
        }

        public static readonly DependencyProperty OnAfterCharacterRenderProperty =
            DependencyProperty.Register(nameof(OnAfterCharacterRender), typeof(AfterCharacterRender), typeof(ColoredLabel));

        
        public AfterRender OnAfterRender
        {
            get => (AfterRender)GetValue(OnAfterRenderProperty);
            set => SetValue(OnAfterRenderProperty, value);
        }

        public static readonly DependencyProperty OnAfterRenderProperty =
            DependencyProperty.Register(nameof(OnAfterRender), typeof(AfterRender), typeof(ColoredLabel));
        
        #endregion
        
        
        private void ColoredLabel_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextChanged += ColoredLabel_OnTextChanged;
        }
        
        public int GetLineCount()
        {
            return GetLines().Length;
        }

        public List<SyntaxChar>[] GetLines(bool omitNewLines = false)
        {
            List<List<SyntaxChar>> lines = new List<List<SyntaxChar>> { new List<SyntaxChar>() };
            int currentIndex = 0;

            foreach (var character in Characters)
            {
                if (character.Value == ColoredLabel.NewLine)
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

            var arr = lines.ToArray();
            lines = null; // Memory allocation issue fixed?

            return arr;
        }

        /*internal int GetLineCount()
        {
            return Characters.Count(c => c.Value == NewLine) + 1;
        }

        public List<SyntaxChar>[] GetLines(bool omitNewLines = false)
        {
            List<List<SyntaxChar>> lines = new List<List<SyntaxChar>> { new List<SyntaxChar>() };
            int currentIndex = 0;

            foreach (var character in Characters)
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

            var arr = lines.ToArray();
            lines = null; // Memory allocation issue fixed?

            return arr;
        }*/

        private void ColoredLabel_OnTextChanged(object sender, RoutedEventArgs e)
        {
            Scheme?.Highlight(this);
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var line = 0;

            var lineWidth = 0d;
            var index = 0;
            
            OnBeforeRender?.Invoke(drawingContext);

            foreach (var character in Characters)
            {
                if (character.Value == NewLine)
                {
                    line++;
                    lineWidth = 0d;
                    continue;
                }

                var charPos = new Point(lineWidth, line * LineHeight);
                var charSize = character.Size;
                
                var charRect = new Rect(charPos, charSize);
                
                OnBeforeCharacterRender?.Invoke(drawingContext, charRect, index);

                character.Draw(drawingContext, charPos);
                
                OnAfterCharacterRender?.Invoke(drawingContext, charRect, index);

                lineWidth += charSize.Width;
                index++;
            }
            
            OnAfterRender?.Invoke(drawingContext);
        }
    }
}