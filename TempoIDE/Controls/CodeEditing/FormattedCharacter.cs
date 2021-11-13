using System.Windows;
using System.Windows.Media;
using TempoControls.Core.InfoStructs;

namespace TempoIDE.Controls.CodeEditing
{
    public readonly struct FormattedCharacter : IFormattedVisual
    {
        public char Character { get; }
        public DrawInfo DrawInfo { get; }

        public FormattedCharacter(char character, DrawInfo drawInfo)
        {
            Character = character;
            DrawInfo = drawInfo;
        }

        public override string ToString()
        {
            return Character.ToString();
        }

        public double Draw(DrawingContext context, Point point)
        {
            return new FormattedString(this, DrawInfo, DrawInfo.Foreground).Draw(context, point);
        }
    }
}