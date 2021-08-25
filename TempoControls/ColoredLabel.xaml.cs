using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace TempoControls
{
    public partial class ColoredLabel : UserControl
    {
        public string Text => TextBuilder.ToString();
        public readonly StringBuilder TextBuilder = new();
        
        public int LineCount => TextBuilder.ToString().Count(c => c == NewLine) + 1;
        
        public int LineHeight { get; set; } = 15;
        public Typeface Typeface { get; set; } = new Typeface("Verdana");
        public new int FontSize { get; set; } = 14;

        public event RoutedEventHandler TextChanged;

        public const char NewLine = '\n';

        public ISyntaxScheme Scheme { get; private set; }
        public ICompletionProvider CompletionProvider { get; private set; }

        public ColoredLabel()
        {
            InitializeComponent();
        }
        
        public delegate void BeforeRenderHandler(DrawingContext context);
        public delegate void BeforeCharacterReadHandler(DrawingContext context, Rect rect, int index);
        public delegate void AfterCharacterReadHandler(DrawingContext context, Rect rect, int index);
        public delegate void AfterHighlightHandler(SyntaxCharCollection formattedText);
        public delegate void AfterRenderHandler(DrawingContext context);
        
        public event BeforeRenderHandler BeforeRender;
        public event BeforeCharacterReadHandler BeforeCharacterRead;
        public event AfterCharacterReadHandler AfterCharacterRead;
        public event AfterHighlightHandler AfterHighlight;
        public event AfterRenderHandler AfterRender;


        private void ColoredLabel_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextChanged += ColoredLabel_OnTextChanged;
        }
        
        private void ColoredLabel_OnTextChanged(object sender, RoutedEventArgs e)
        {
            TextBuilder.Replace("\r", ""); // Remove \r to prevent incompatibilities
            InvalidateMeasure();
        }
        
        protected override Size MeasureOverride(Size constraint)
        {
            var longestLine = GetLines().OrderByDescending(line => line.Length).FirstOrDefault();
            var totalWidth = CreateFormattedText(longestLine).WidthIncludingTrailingWhitespace;
            
            return new Size(totalWidth, LineHeight * LineCount);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            BeforeRender?.Invoke(drawingContext);
            
            base.OnRender(drawingContext);

            var index = 0;
            var lineCount = 0;
            var lineWidth = 0d;
            
            var characters = new SyntaxCharCollection();
            var lines = new List<SyntaxCharCollection> { new() };

            for (; index < TextBuilder.Length; index++)
            {
                var character = new SyntaxChar(TextBuilder[index], DefaultDrawInfo);
                var charPos = new Point(lineWidth, lineCount * LineHeight);
                var charSize = character.Size;
                var charRect = new Rect(charPos, charSize);

                BeforeCharacterRead?.Invoke(drawingContext, charRect, index);
                
                characters.Add(character);
                lines[lineCount].Add(character);

                AfterCharacterRead?.Invoke(drawingContext, charRect, index);

                lineWidth += charSize.Width;

                if (character.Value == NewLine)
                {
                    lineCount++;
                    lineWidth = 0d;
                    
                    lines.Add(new SyntaxCharCollection());
                }
            }
            
            Scheme.Highlight(this, characters);
            AfterHighlight?.Invoke(characters);

            for (var lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                var line = lines[lineIndex];
                var formatted = CreateFormattedText(line.ToString());

                for (var i = 0; i < line.Count; i++)
                {
                    var syntaxChar = line[i];
                    formatted.SetForegroundBrush(syntaxChar.Foreground, i, 1);
                }

                new ColoredTextLine(formatted).Draw(drawingContext, new Point(0, lineIndex * LineHeight));
            }
            
            AfterRender?.Invoke(drawingContext);
        }

        internal FormattedText CreateFormattedText(string text)
        {
            return new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                Typeface,
                FontSize,
                Brushes.White,
                TextDpi)
            {
                LineHeight = LineHeight
            };
        }
    }
}