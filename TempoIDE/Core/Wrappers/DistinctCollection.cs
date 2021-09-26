using System.Collections;
using System.Collections.Generic;

namespace TempoIDE.Core.Wrappers
{
    public class DistinctCollection<T> : ICollection<T>
    {
        private readonly List<T> list = new();

        public int Count => list.Count;
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (list.Contains(item))
                return;
            
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array);
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}