using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoControls.CompletionProviders;
using TempoControls.Core.Types;
using TempoControls.SyntaxSchemes;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace TempoControls.Controls
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

        public Rect CullingRange = Rect.Empty;

        public int LineCount => GetLines().Length;
        
        public int LineHeight { get; set; } = 15;
        public new int FontSize { get; set; } = 14;

        public event RoutedEventHandler TextChanged;

        public const char NewLine = '\n'; // TODO: Does not work with foreign newlines like \r or \n\r

        internal ISyntaxScheme Scheme { get; private set; }
        internal ICompletionProvider CompletionProvider { get; private set; }
        internal readonly List<SyntaxChar> Characters = new();

        public ColoredLabel()
        {
            InitializeComponent();
        }
        
        public delegate void BeforeRenderDel(DrawingContext context);
        public delegate void BeforeCharacterRenderDel(DrawingContext context, Rect rect, int index);
        public delegate void AfterCharacterRenderDel(DrawingContext context, Rect rect, int index);
        public delegate void AfterRenderDel(DrawingContext context);
        
        public event BeforeRenderDel OnBeforeRender;
        public event BeforeCharacterRenderDel OnBeforeCharacterRender;
        public event AfterCharacterRenderDel OnAfterCharacterRender;
        public event AfterRenderDel OnAfterRender;


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
            
            OnBeforeRender?.Invoke(drawingContext);

            var startIndex = -1;
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

                if (CullingRange != Rect.Empty)
                {
                    // Left and Right are always 0 for some reason
                    if (charPos.Y < CullingRange.Top)
                        continue;
                    if (charPos.Y > CullingRange.Bottom)
                        break;
                }

                if (startIndex == -1)
                    startIndex = index;

                OnBeforeCharacterRender?.Invoke(drawingContext, charRect, index);
                
                text += character.Value;
                
                OnAfterCharacterRender?.Invoke(drawingContext, charRect, index);

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

            if (startIndex == -1)
                startIndex = 0;

            formatted.LineHeight = LineHeight;
            Scheme.Highlight(this, new HighlightInfo(formatted, new IntRange(startIndex, index)));

            drawingContext.DrawText(formatted, new Point(0, 0));

            OnAfterRender?.Invoke(drawingContext);
        }
        
        
        private GlyphRun CreateGlyphRun(string text, Point origin)
        {
            var glyphTypeface = new GlyphTypeface(new Uri("file:///C:\\WINDOWS\\FONTS\\ARIAL.TTF"));

            var glyphIndices = new ushort[text.Length];
            var advanceWidths = new double[text.Length];

            for (var index = 0; index < text.Length; index++)
            {
                var glyphIndex = (ushort)(text[index] - 29);
                glyphIndices[index] = glyphIndex;
                advanceWidths[index] = glyphTypeface.AdvanceWidths[glyphIndex] * FontSize;
            }
            
            return new GlyphRun(
                glyphTypeface, 0, false, 
                FontSize, glyphIndices, origin, advanceWidths, 
                null, null, null,
                null, null, null
            );
        }
    }
}