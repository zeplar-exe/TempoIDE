using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TempoControls.Core.Types.Collections
{
    public interface IColoredLabelLine
    {
        public void Draw(DrawingContext context, Point origin);
    }

    public class ColoredTextLine : IColoredLabelLine
    {
        private readonly SyntaxCharCollection characters;
        private readonly DrawInfo drawInfo;

        public ColoredTextLine(SyntaxCharCollection characters, DrawInfo drawInfo)
        {
            this.characters = characters;
            this.drawInfo = drawInfo;
        }

        public void Draw(DrawingContext context, Point origin)
        {
            var formatted = CreateFormattedText(characters.ToString());
            var lineDrawInfos = new List<RectangleDrawInfo>();

            for (var index = 0; index < characters.Count; index++)
            {
                var character = characters[index];
                
                formatted.SetForegroundBrush(character.Foreground, index, 1);

                if (character.UnderlineColor == Brushes.Transparent)
                    continue;
                
                if (character.UnderlineType == UnderlineType.Straight)
                {
                    lineDrawInfos.Add(new RectangleDrawInfo(
                        character.UnderlineColor,
                        new Rect(
                            character.Size.Width * index,
                            origin.Y + character.Size.Height - 5, // TODO: Try to make this 5 not constant
                            character.Size.Width, 1)));
                }
                else if (character.UnderlineType == UnderlineType.Squiggly)
                {
                        
                }
            }
            
            context.DrawText(formatted, origin);

            foreach (var line in lineDrawInfos)
            {
                context.DrawRectangle(line.Brush, null, line.Rect);
            }
        }
        
        private FormattedText CreateFormattedText(string text)
        {
            return new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                drawInfo.Typeface,
                drawInfo.FontSize,
                Brushes.White,
                drawInfo.Dpi)
            {
                LineHeight = drawInfo.LineHeight
            };
        }
    }

    public class CsMetricLine : IColoredLabelLine
    {
        public void Draw(DrawingContext context, Point origin)
        {
            
        }
    }

    public readonly struct RectangleDrawInfo
    {
        public readonly Brush Brush;
        public readonly Rect Rect;

        public RectangleDrawInfo(Brush brush, Rect rect)
        {
            Brush = brush;
            Rect = rect;
        }
    }
}