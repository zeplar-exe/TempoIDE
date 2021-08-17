using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoControls.Core.Types;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace TempoControls
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
        
        public int LineCount => GetLines().Length;
        
        public int LineHeight { get; set; } = 15;
        public new int FontSize { get; set; } = 14;

        public event RoutedEventHandler TextChanged;

        public const char NewLine = '\n'; // TODO: Does not work with foreign newlines like \r or \n\r

        public ISyntaxScheme Scheme { get; private set; }
        public ICompletionProvider CompletionProvider { get; private set; }
        internal readonly List<SyntaxChar> Characters = new();

        public ColoredLabel()
        {
            InitializeComponent();
        }
        
        public delegate void BeforeRenderHandler(DrawingContext context);
        public delegate void BeforeCharacterRenderHandler(DrawingContext context, Rect rect, int index);
        public delegate void AfterCharacterRenderHandler(DrawingContext context, Rect rect, int index);
        public delegate void AfterRenderHandler(DrawingContext context);
        
        public event BeforeRenderHandler BeforeRender;
        public event BeforeCharacterRenderHandler BeforeCharacterRender;
        public event AfterCharacterRenderHandler AfterCharacterRender;
        public event AfterRenderHandler AfterRender;


        private void ColoredLabel_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextChanged += ColoredLabel_OnTextChanged;
        }
        
        private void ColoredLabel_OnTextChanged(object sender, RoutedEventArgs e)
        {
            InvalidateMeasure();
        }
        
        protected override Size MeasureOverride(Size constraint)
        {
            var longestLine = GetLines().OrderByDescending(line => line.Count).FirstOrDefault();
            var totalWidth = longestLine?.TotalWidth ?? 0;
            
            return new Size(totalWidth, LineHeight * LineCount);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            
            BeforeRender?.Invoke(drawingContext);
            
            var index = 0;
            var line = 0;
            var lineWidth = 0d;

            var text = "";

            for (; index < Characters.Count; index++)
            {
                var character = Characters[index];
                var charPos = new Point(lineWidth, line * LineHeight);
                var charSize = character.Size;

                var charRect = new Rect(charPos, charSize);

                BeforeCharacterRender?.Invoke(drawingContext, charRect, index);
                
                text += character.Value;
                
                AfterCharacterRender?.Invoke(drawingContext, charRect, index);

                lineWidth += charSize.Width;

                if (character.Value == NewLine)
                {
                    line++;
                    lineWidth = 0d;
                }
            }
            
            var formatted = new FormattedText(
                text, 
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                FontSize, Brushes.White, 
                GetTextDpi());

            formatted.LineHeight = LineHeight;
            
            Scheme.Highlight(this, formatted);

            drawingContext.DrawText(formatted, new Point(0, 0));

            AfterRender?.Invoke(drawingContext);
        }
    }
}