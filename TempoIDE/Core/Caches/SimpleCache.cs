using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace TempoIDE.Core.Caches
{
    public class SimpleCache<TK, TV> : IEnumerable<KeyValuePair<TK, CacheItem<TV>>>
    {
        private readonly Dictionary<TK, CacheItem<TV>> items = new();
        private readonly CacheOptions options;

        public IEnumerable<TK> Keys => items.Keys;
        public IEnumerable<TV> Values => items.Values.Select(v => v.Item);

        public long FreeSpace
        {
            get
            {
                var size = options.MaximumSize;
                
                if (size == null)
                    return -1;

                return (long)size - items.Values.Sum(i => (long)i.Size);
            }
        }

        public SimpleCache(CacheOptions options = null)
        {
            this.options = options ?? new CacheOptions();
        }
        
        public bool KeyExists(TK key) => items.ContainsKey(key);

        public void Set(TK key, TV value)
        {
            Set(key, new CacheItem<TV>(value, options.DefaultItemOptions));
        }
        
        public void Set(TK key, CacheItem<TV> value)
        {
            var freeAfterInsertion = FreeSpace - (long)value.Size;

            if (freeAfterInsertion < 0 && options.MakeRoomOnSizeLimit)
            {
                foreach (var item in items.ToDictionary(
                    entry => entry.Key, entry => entry.Value))
                {
                    freeAfterInsertion += (long)item.Value.Size;
                    
                    if (freeAfterInsertion >= 0)
                        break;
                }
                
                if (freeAfterInsertion < 0)
                    return;
            }

            items[key] = value;
        }

        public TV Get(TK key)
        {
            var item = items[key];
            item.Renew();
            
            return item.Item;
        }

        public TV GetOrCreate(TK key, TV fallback)
        {
            return GetOrCreate(key, new CacheItem<TV>(fallback, options.DefaultItemOptions));
        }
        
        public TV GetOrCreate(TK key, CacheItem<TV> fallback)
        {
            if (items.TryGetValue(key, out var item))
            {
                item.Renew();
                
                return item.Item;
            }
            
            Set(key, fallback);

            return Get(key);
        }

        public bool Remove(TK key) => items.Remove(key);

        public void Clear() => items.Clear();

        public IEnumerator<KeyValuePair<TK, CacheItem<TV>>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CacheItem<T>
    {
        private CacheItemOptions options;
        internal readonly Timer Timer;

        public event EventHandler DeletionRequested;

        public readonly T Item;
        public ulong Size => options?.Size ?? 0;

        public CacheItem(T item, CacheItemOptions options = null)
        {
            Item = item;

            options ??= new CacheItemOptions();
            this.options = options;

            if (options.NonAccessDeletionTime != TimeSpan.Zero)
            {
                Timer = new Timer(options.NonAccessDeletionTime.TotalMilliseconds);
                Timer.Elapsed += delegate { DeletionRequested?.Invoke(this, EventArgs.Empty); };

                Timer.Start();
            }
        }

        public void Renew()
        {
            if (Timer == null)
                return;
            
            Timer.Stop();
            Timer.Start();
        }
    }

    public class CacheOptions
    {
        public ulong? MaximumSize;
        public bool MakeRoomOnSizeLimit;
        public CacheItemOptions DefaultItemOptions;
    }

    public class CacheItemOptions
    {
        public TimeSpan NonAccessDeletionTime;
        public ulong Size;
    }
}