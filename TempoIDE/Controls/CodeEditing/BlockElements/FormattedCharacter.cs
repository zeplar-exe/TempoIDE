using System.Collections.Generic;
using System.Windows.Media;
using TempoControls.Core.InfoStructs;

namespace TempoIDE.Controls.CodeEditing.BlockElements;

public class FormattedCharacter
{
    private List<FormattedCharacterVisual> b_visuals;

    public IEnumerable<FormattedCharacterVisual> Visuals => b_visuals;
    public char Character { get; }

    public FormattedCharacter(char character)
    {
        b_visuals = new List<FormattedCharacterVisual>();
        Character = character;
    }
    
    public FormattedText CreateFormattedText(DrawInfo drawInfo)
    {
        return new FormattedString(this, drawInfo).CreateFormattedText();
    }

    public override string ToString()
    {
        return Character.ToString();
    }
}