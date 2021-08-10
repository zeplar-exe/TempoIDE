using System.Windows.Media;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.SyntaxSchemes
{
    public class DefaultSyntaxScheme : ISyntaxScheme
    {
        public Brush Default => Brushes.White;

        public void Highlight(ColoredLabel label)
        {
            
        }
        
        public AutoCompletion[] GetAutoCompletions(SyntaxTextBox label)
        {
            return null;
        }
    }
}