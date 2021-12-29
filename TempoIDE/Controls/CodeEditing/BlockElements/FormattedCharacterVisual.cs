using System;
using System.Windows;
using TempoControls.Core.IntTypes;

namespace TempoIDE.Controls.CodeEditing.BlockElements;

public class FormattedCharacterVisual
{
    private Point b_overridenPosition;

    public Point OverridenPosition
    {
        get
        {
            if (Position != FormattedVisualPosition.Override)
                throw new InvalidOperationException(
                    "The visual's position must be set to 'Override' in order to access its OverridenPosition.");

            return b_overridenPosition;
        }
        private set
        {
            if (Position != FormattedVisualPosition.Override)
                throw new InvalidOperationException(
                    "The visual's position must be set to 'Override' in order to use the OverridePosition method.");
        
            b_overridenPosition = value;
        }
    }

    public FormattedVisualPosition Position { get; }
    public FormattedVisual Visual { get; }
    
    public FormattedCharacterVisual(FormattedVisualPosition position, FormattedVisual visual)
    {
        Position = position;
        Visual = visual;
    }

    public void OverridePosition(Point position)
    {
        OverridenPosition = position;
    }
}