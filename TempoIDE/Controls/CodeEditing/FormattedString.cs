using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TempoControls.Core.InfoStructs;

namespace TempoIDE.Controls.CodeEditing
{
    public class FormattedString : IFormattedVisual, IEnumerable<FormattedCharacter>
    {
        private List<FormattedCharacter> Characters { get; } = new();

        public FormattedText FormattedText { get; private set; }
        public DrawInfo DrawInfo { get; }
        public Brush DefaultColor { get; }

        public FormattedString(FormattedCharacter character, DrawInfo drawInfo, Brush defaultColor)
        {
            DrawInfo = drawInfo;
            DefaultColor = defaultColor;
           
            Characters.Add(character);
            FormattedText = CreateFormattedText();
        }
        
        public FormattedString(IEnumerable<FormattedCharacter> characters, DrawInfo drawInfo, Brush defaultColor)
        {
            DrawInfo = drawInfo;
            DefaultColor = defaultColor;
            
            Characters.AddRange(characters);
            FormattedText = CreateFormattedText();
        }

        public FormattedString(string text, DrawInfo drawInfo, Brush defaultColor)
        {
            DrawInfo = drawInfo;
            DefaultColor = defaultColor;
            
            Characters.AddRange(text.Select(c => new FormattedCharacter(c, DrawInfo)));
            FormattedText = CreateFormattedText();
        }
        
        public void Draw(DrawingContext context, Point point)
        {
            context.DrawText(FormattedText, point);
        }

        private FormattedText CreateFormattedText()
        {
            var formatted = new FormattedText(
                ToString(), CultureInfo.CurrentCulture, 
                FlowDirection.LeftToRight, DrawInfo.Typeface,
                DrawInfo.FontSize, DefaultColor,
                DrawInfo.Dpi)
            {
                LineHeight = DrawInfo.LineHeight
            };
            
            for (var index = 0; index < Characters.Count; index++)
            {
                var character = Characters[index];
                
                formatted.SetForegroundBrush(character.DrawInfo.Foreground, index, 1);
            }

            return formatted;
        }
        
        public IEnumerator<FormattedCharacter> GetEnumerator()
        {
            return Characters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Concat(Characters.Select(c => c.ToString()));
        }
    }
}