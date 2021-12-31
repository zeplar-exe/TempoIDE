using System;
using System.Collections.Generic;
using System.Windows.Media;
using TempoControls.Core.InfoStructs;

namespace TempoIDE.Controls.CodeEditing.BlockElements;

public class FormattedCharacter : IEquatable<FormattedCharacter>
{
    private readonly List<FormattedCharacterVisual> b_visuals;

    public IEnumerable<FormattedCharacterVisual> Visuals => b_visuals;
    public char Character { get; }

    public FormattedCharacter(char character)
    {
        b_visuals = new List<FormattedCharacterVisual>();
        Character = character;
    }

    public void AddVisual(FormattedCharacterVisual visual) => b_visuals.Add(visual);
    public bool RemoveVisual(FormattedCharacterVisual visual) => b_visuals.Remove(visual);

    public FormattedText CreateFormattedText(DrawInfo drawInfo)
    {
        return new FormattedString(this, drawInfo).CreateFormattedText();
    }

    public bool Equals(FormattedCharacter? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        
        return b_visuals.Equals(other.b_visuals) && Character == other.Character;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        
        return Equals((FormattedCharacter)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(b_visuals, Character);
    }

    public static bool operator ==(FormattedCharacter? left, FormattedCharacter? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(FormattedCharacter? left, FormattedCharacter? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return Character.ToString();
    }
}