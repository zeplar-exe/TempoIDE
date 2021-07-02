using System;
using System.Collections.Generic;
using System.Linq;

namespace TempoIDE.Classes
{
    public class OrderedDictionary<TK, TV> : List<KeyValuePair<TK, TV>>
    {
        public void Add(TK key, TV value)
        {
            if (ContainsKey(key))
                throw new ArgumentException($"Key '{key}' already exists.");
            
            Add(new KeyValuePair<TK, TV>(key, value));
        }

        public void Remove(TK key)
        {
            foreach (var pair in this.Where(pair => pair.Key.Equals(key)))
            {
                Remove(pair);
                break;
            }
        }

        public TV this[TK key]
        {
            get
            {
                foreach (var pair in this.Where(pair => pair.Key.Equals(key)))
                    return pair.Value;

                throw new KeyNotFoundException($"Key '{key}' does not exist.");
            }
        }

        public int IndexOf(TK key)
        {
            foreach (var pair in this.Where(pair => pair.Key.Equals(key)))
                return IndexOf(pair);
            
            throw new KeyNotFoundException($"Key '{key}' does not exist.");
        }

        public bool ContainsKey(TK key)
        {
            try
            {
                var _ = this[key];
            }
            catch (KeyNotFoundException)
            {
                return false;
            }

            return true;
        }
    }
}