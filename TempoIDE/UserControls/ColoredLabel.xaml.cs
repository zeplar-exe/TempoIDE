using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Classes.SyntaxSchemes;
using TempoIDE.Classes.Types;

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

        public int LineCount => GetLines().Length;
        
        public int LineHeight = 15;
        public new int FontSize = 14;

        public event RoutedEventHandler TextChanged;

        public const char NewLine = '\n'; // TODO: Does not work with foreign newlines like \r or \n\r

        internal ISyntaxScheme Scheme { get; private set; }
        internal readonly List<SyntaxChar> Characters = new List<SyntaxChar>();

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
            Scheme?.Highlight(this);
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
            
            var line = 0;

            var lineWidth = 0d;
            var index = 0;
            
            OnBeforeRender?.Invoke(drawingContext);
            
            foreach (var character in Characters)
            {
                var charPos = new Point(lineWidth, line * LineHeight);
                var charSize = character.Size;
                
                var charRect = new Rect(charPos, charSize);
                
                OnBeforeCharacterRender?.Invoke(drawingContext, charRect, index);

                drawingContext.DrawText(character.FormattedText, charPos);

                OnAfterCharacterRender?.Invoke(drawingContext, charRect, index);

                lineWidth += charSize.Width;
                index++;
                
                if (character.Value == NewLine)
                {
                    line++;
                    lineWidth = 0d;
                }
            }
            
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