using System.Windows.Media;
using TempoControls.Core.Types.Collections;

namespace TempoControls.Core.SyntaxSchemes
{
    public interface ISyntaxScheme
    {
        public Brush Default { get; }
        
        public void Highlight(ColoredLabel label, SyntaxCharCollection characters);
    }
    
    public class DefaultSyntaxScheme : ISyntaxScheme
    {
        public Brush Default => Brushes.White;

        public void Highlight(ColoredLabel label, SyntaxCharCollection processedText)
        {
            
        }
    }

    public interface IProgrammingLanguageSyntaxScheme : ISyntaxScheme
    {
        public Brush Number { get; }
        public Brush String { get; }
        public Brush Comment { get; }
        public Brush Keyword { get; }
        public Brush Type { get; }
        public Brush Method { get; }
        public Brush Member { get; }
    }
}