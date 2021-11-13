using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Jammo.ParserTools;

namespace TempoIDE.Controls.CodeEditing
{
    public abstract class FormattedBlock : IFormattedVisual
    {
        private readonly List<IFormattedVisual> visuals = new();
        
        public IEnumerable<IFormattedVisual> Visuals => visuals;

        public IndexSpan Span { get; }
        
        protected FormattedBlock(IndexSpan span)
        {
            Span = span;
        }
        
        public double Draw(DrawingContext context, Point point)
        {
            throw new System.NotImplementedException();
        }
    }
}