using System.Windows.Media;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.SyntaxSchemes
{
    public static class SyntaxSchemeFactory
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

        public void Highlight(ColoredLabel label);
        
        public AutoCompletion[] GetAutoCompletions(SyntaxTextBox label);
    }

    public interface IProgrammingLanguageSyntaxScheme : ISyntaxScheme
    {
        public Brush Number { get; }
        public Brush String { get; }
        public Brush Comment { get; }
        public Brush Identifier { get; }
        public Brush Type { get; }
        public Brush Method { get; }
        public Brush Member { get; }
    }
}