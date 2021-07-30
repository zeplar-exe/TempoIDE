using System.Collections.Generic;

namespace TempoIDE.Classes.Types
{
    public class SyntaxCharCollection : List<SyntaxChar>
    {
        public double TotalWidth;

        public new void Add(SyntaxChar item)
        {
            TotalWidth += item.Size.Width;
            
            base.Add(item);
        }

        public new void Insert(int index, SyntaxChar item)
        {
            TotalWidth += item.Size.Width;

            base.Insert(index, item);
        }

        public new void Remove(SyntaxChar item)
        {
            TotalWidth -= item.Size.Width;

            base.Remove(item);
        }

        public new void Clear()
        {
            TotalWidth = 0;
            
            base.Clear();
        }
    }
}