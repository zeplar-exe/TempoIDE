using System.Collections.Generic;

namespace TempoIDE.Core.DataStructures;

public class Bit
{
    private readonly bool value;
        
    public readonly List<Bit> Children = new();

    public Bit(bool value)
    {
        this.value = value;
    }
        
    public bool ToBool()
    {
        return value;
    }

    public IEnumerable<Bit> EnumerateTree()
    {
        foreach (var child in Children)
        {
            yield return child;
                
            foreach (var nested in child.EnumerateTree())
                yield return nested;
        }
    }
}