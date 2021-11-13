using System.Windows.Media;
using TempoControls.Core.InfoStructs;

namespace TempoIDE.Controls
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

        public double Draw(DrawingVisual visual)
        {
            return new FormattedString(this, DrawInfo, DrawInfo.Foreground).Draw(visual);
        }
    }
}