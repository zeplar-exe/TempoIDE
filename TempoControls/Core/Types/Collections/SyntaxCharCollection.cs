using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TempoControls.Core.Types.Collections
{
    public class SyntaxCharCollection : ICollection<SyntaxChar>
    {
        private List<SyntaxChar> items = new();

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public double TotalWidth { get; private set; }

        public SyntaxChar this[int index]
        {
            get => items[index];
            set
            {
                TotalWidth -= items[index].Size.Width;
                items[index] = value;
            }
        }

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

        public override string ToString()
        {
            return string.Concat(items.Select(i => i.Value));
        }
    }
}