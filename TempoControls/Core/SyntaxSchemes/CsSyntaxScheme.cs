using System.Windows.Media;
using Jammo.CsAnalysis.Helpers;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;

namespace TempoControls.Core.SyntaxSchemes
{
    public class CsSyntaxScheme : IProgrammingLanguageSyntaxScheme
    {
        public Brush Default => Brushes.White;
        public Brush String => Brushes.SeaGreen;
        public Brush Number => Brushes.LightCoral;
        public Brush Comment => Brushes.ForestGreen;
        public Brush Keyword => Brushes.CornflowerBlue;
        public Brush Type => Brushes.MediumPurple;
        public Brush Method => Brushes.LightGreen;
        public Brush Member => Brushes.CadetBlue;

        public void Highlight(ColoredLabel label, SyntaxCharCollection characters)
        {
            var reader = new SpecialSyntaxReader(label.Text);
            
            foreach (var keyword in reader.Keywords)
            {
                characters.UpdateForeground(
                    new IntRange(keyword.Span.Start, keyword.Span.End),
                    Keyword);
            }
        }
    }
}