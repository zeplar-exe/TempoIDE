using System.Windows.Media;

namespace TempoControls
{
    public interface ISyntaxScheme
    {
        public Brush Default { get; }

        public void Highlight(ColoredLabel label, FormattedText processed);
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