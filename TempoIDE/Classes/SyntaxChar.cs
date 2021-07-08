using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TempoIDE.Classes
{
    // TODO: Set this back to a struct if performance dwindles
    public readonly struct SyntaxChar
    {
        public readonly char Value;
        public readonly FormattedText FormattedText;

        public readonly Brush ForegroundBrush;
        public readonly Size Size;

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
}