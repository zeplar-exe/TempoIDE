using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TempoIDE.Classes
{
    // TODO: Set this back to a struct if performance dwindles
    public class SyntaxChar
    {
        public char Value;
        public FormattedText FormattedText;

        public Brush ForegroundBrush;
        public Size Size;

        public SyntaxChar(char value, CharDrawInfo info)
        {
            Value = value;
            ForegroundBrush = info.Foreground;

            FormattedText = new FormattedText(
                Value.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                info.Typeface,
                info.FontSize,
                info.Foreground,
                info.Dpi
            );
            
            Size = new Size(FormattedText.WidthIncludingTrailingWhitespace, FormattedText.Height);
        }

        public Size Draw(DrawingContext context, Point point)
        {
            context.DrawText(
                FormattedText,
                point
            );

            return Size;
        }
        
        public static explicit operator char(SyntaxChar syntaxChar) => syntaxChar.Value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public readonly struct CharDrawInfo
    {
        public readonly Brush Foreground;
        public readonly int FontSize;
        public readonly Typeface Typeface;
        public readonly double Dpi;

        public CharDrawInfo(int fontSize, Typeface typeface, double dpi, Brush foreground)
        {
            FontSize = fontSize;
            Typeface = typeface;
            Dpi = dpi;
            Foreground = foreground;
        }
    }
}