using System;
using System.Timers;

namespace TempoIDE.Core.Caches;

public class CacheItem<T>
{
    private CacheItemOptions Options { get; }
    internal Timer? Timer { get; }

    public event EventHandler? DeletionRequested;

    public T Item { get; }
    public ulong Size => Options?.Size ?? 0;

    public CacheItem(T item, CacheItemOptions? options = null)
    {
        Item = item;

        options ??= new CacheItemOptions();
        Options = options;

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