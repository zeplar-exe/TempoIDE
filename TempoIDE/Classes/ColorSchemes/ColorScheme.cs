using System.Windows.Media;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.ColorSchemes
{
    public static class ColorScheme
    {
        public static IColorScheme GetColorSchemeByExtension(string extension)
        {
            switch (extension.Replace(".", string.Empty))
            {
                case "cs":
                    return new CsColorScheme();
                default:
                    return new DefaultColorScheme();
            }
        }
    }

    public interface IColorScheme
    {
        public Brush Default { get; }
        public Brush Number { get; }
        public Brush Comment { get; }
        public Brush Identifier { get; }
        public Brush Type { get; }
        public Brush Method { get; }
        public Brush Member { get; }

        public void Highlight(ref SyntaxTextBox textBox);
    }
}