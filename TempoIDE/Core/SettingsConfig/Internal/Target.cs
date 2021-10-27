using System;

namespace TempoIDE.Core.SettingsConfig.Internal
{
    public class Target<T>
    {
        private T[] items;
        private int index = -1;

        public T Current
        {
            get
            {
                if (!IterationStarted)
                    throw new IndexOutOfRangeException("Iteration has not started.");

                return items[index];
            }
        }

        public bool IterationStarted => index > -1;

        public bool AtEnd => index == items.Length - 1;

        public Target(T[] items)
        {
            this.items = items;
        }

        public bool TryMoveNext(out T result)
        {
            result = default;
            
            if (AtEnd)
                return false;

            result = items[++index];
            
            return true;
        }

        public void Reset() => index = 0;
    }
}