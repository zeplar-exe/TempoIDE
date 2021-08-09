using System.Collections;
using System.Collections.Generic;

namespace TempoIDE.Classes.Types
{
    public class SyntaxCharCollection : IReadOnlyCollection<SyntaxChar>
    {
        private List<SyntaxChar> items = new();

        public int Count => items.Count;
        public bool IsReadOnly => false;
        
        public double TotalWidth;

        public SyntaxChar this[int index] => items[index];

        public void Add(SyntaxChar item)
        {
            TotalWidth += item.Size.Width;
            
            items.Add(item);
        }

        public void Insert(int index, SyntaxChar item)
        {
            TotalWidth += item.Size.Width;
            
            items.Insert(index, item);
        }
        
        public bool Remove(SyntaxChar item)
        {
            TotalWidth -= item.Size.Width;
            
            return items.Remove(item);
        }

        public void Clear()
        {
            TotalWidth = 0;
            
            items.Clear();
        }

        public bool Contains(SyntaxChar item)
        {
            return items.Contains(item);
        }

        public void CopyTo(SyntaxChar[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }
        
        public IEnumerator<SyntaxChar> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}