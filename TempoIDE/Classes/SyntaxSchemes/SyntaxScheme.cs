using System.Windows.Media;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.SyntaxSchemes
{
    public static class ColorScheme
    {
        public static ISyntaxScheme GetColorSchemeByExtension(string extension)
        {
            switch (extension.Replace(".", string.Empty))
            {
                case "cs":
                    return new CsSyntaxScheme();
                default:
                    return new DefaultSyntaxScheme();
            }
        }
    }

    public interface ISyntaxScheme
    {
        public Brush Default { get; }

        public void Highlight(ColoredLabel textBox);
        
        public AutoCompletion[] GetAutoCompletions(SyntaxTextBox textBox);
    }

    public interface IProgrammingLanguageColorScheme : ISyntaxScheme
    {
        public Brush Number { get; }
        public Brush Comment { get; }
        public Brush Identifier { get; }
        public Brush Type { get; }
        public Brush Method { get; }
        public Brush Member { get; }
    }
}