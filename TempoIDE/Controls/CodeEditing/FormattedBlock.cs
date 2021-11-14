using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace TempoIDE.Controls.CodeEditing
{
    public abstract class FormattedBlock : IFormattedVisual
    {
        private readonly List<IFormattedVisual> visuals = new();
        
        public IEnumerable<IFormattedVisual> Visuals => visuals;

        public abstract void Draw(DrawingContext context, Point point);
        public abstract Size CalculateSize();
    }
}