using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace TempoControls.Core.Types.Collections
{
    public class SyntaxCharCollection : ICollection<SyntaxChar>
    {
        private readonly List<SyntaxChar> items = new();

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public double TotalWidth { get; private set; }

        public SyntaxChar this[int index]
        {
            get => items[index];
            set => Insert(index, value);
        }

        public static SyntaxCharCollection FromString(string text, DrawInfo drawInfo)
        {
            var collection = new SyntaxCharCollection();

            foreach (var character in text)
            {
                collection.Add(new SyntaxChar(character, drawInfo));
            }

            return collection;
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

        public void RemoveAt(int index)
        {
            var item = items[index];

            TotalWidth -= item.Size.Width;

            items.RemoveAt(index);
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

        public void UpdateForeground(IntRange range, Brush foreground)
        {
            foreach (int index in range)
            {
                items[index].Foreground = foreground;
            }
        }
        
        public void UpdateUnderline(IntRange range, Brush color)
        {
            foreach (int index in range)
            {
                items[index].UnderlineColor = color;
            }
        }
        
        public void UpdateUnderlineType(IntRange range, UnderlineType type)
        {
            foreach (int index in range)
            {
                items[index].UnderlineType = type;
            }
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