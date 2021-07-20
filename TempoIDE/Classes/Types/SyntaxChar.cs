using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TempoIDE.Classes.Types
{
    public readonly struct SyntaxChar
    {
        public readonly char Value;
        public readonly FormattedText FormattedText;
        public readonly Size Size;

        public SyntaxChar(char value, CharDrawInfo info)
        {
            Value = value;

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

        public void Draw(DrawingContext context, Point point)
        {
            context.DrawText(
                FormattedText,
                point
            );
        }
        
        public static explicit operator char(SyntaxChar syntaxChar) => syntaxChar.Value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}