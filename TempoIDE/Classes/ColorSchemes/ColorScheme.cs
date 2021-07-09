using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
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

        public void Highlight(ref SyntaxTextBox textBox);

        public static XDocument GetXDocumentFromString(string data)
        {
            return XDocument.Parse(data);
        }
    }

    public interface IProgrammingLanguageColorScheme : IColorScheme
    {
        public Brush Number { get; }
        public Brush Comment { get; }
        public Brush Identifier { get; }
        public Brush Type { get; }
        public Brush Method { get; }
        public Brush Member { get; }
    }
}