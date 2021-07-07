using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TempoIDE.Classes
{
    public struct SyntaxChar
    {
        public char Value;
        public Brush ForegroundBrush;
        public Brush BackgroundBrush;
        public double Width;

        public SyntaxChar(char value, Brush foreground, Brush background)
        {
            Value = value;
            ForegroundBrush = foreground;
            BackgroundBrush = background;
            Width = 0;
        }

        public double Draw(DrawingContext context, CharDrawInfo info)
        {
            var text = new FormattedText(
                Value.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                info.Typeface,
                info.FontSize,
                ForegroundBrush,
                info.Dpi
            );

            context.DrawText(
                text,
                info.Point
            );
            
            Width = text.WidthIncludingTrailingWhitespace;
            return Width;
        }

        public static explicit operator SyntaxChar(char character) => new SyntaxChar(
            character, 
            Brushes.White,
            Brushes.Transparent
        );

        public static explicit operator char(SyntaxChar syntaxChar) => syntaxChar.Value;

        public override string ToString()
        {
            return Value.ToString();
        }

        public char ToChar()
        {
            return Value;
        }
    }

    public readonly struct CharDrawInfo
    {
        public readonly Point Point;
        public readonly int FontSize;
        public readonly Typeface Typeface;
        public readonly double Dpi;

        public CharDrawInfo(Point point, int fontSize, Typeface typeface, double dpi)
        {
            Point = point;
            FontSize = fontSize;
            Typeface = typeface;
            Dpi = dpi;
        }
    }
}