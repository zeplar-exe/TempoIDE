using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TempoControls.Core.InfoStructs;

namespace TempoIDE.Controls.CodeEditing.BlockElements;

public class FormattedString : IEnumerable<FormattedCharacter>
{
    private List<FormattedCharacter> Characters { get; } = new();
    public DrawInfo DrawInfo { get; }

    public FormattedString(FormattedCharacter character, DrawInfo drawInfo)
    {
        DrawInfo = drawInfo;
           
        Characters.Add(character);
    }
        
    public FormattedString(IEnumerable<FormattedCharacter> characters, DrawInfo drawInfo)
    {
        DrawInfo = drawInfo;
            
        Characters.AddRange(characters);
    }

    public FormattedString(string text, DrawInfo drawInfo)
    {
        DrawInfo = drawInfo;
            
        Characters.AddRange(text.Select(c => new FormattedCharacter(c)));
    }

    public FormattedText CreateFormattedText()
    {
        return new FormattedText(
            ToString(),
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            DrawInfo.Typeface,
            DrawInfo.FontSize,
            DrawInfo.Foreground,
            DrawInfo.Dpi);
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