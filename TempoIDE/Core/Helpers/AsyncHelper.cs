using System;
using System.Threading;
using System.Threading.Tasks;

namespace TempoIDE.Core.Helpers
{
    public static class AsyncHelper
    {
        public static async Task WaitUntilNotNull(Func<object> getter, CancellationToken cancellationToken = new())
        {
            await WaitUntil(() => getter.Invoke() != null, cancellationToken);
        }
        
        public static async Task WaitUntilEqual(Func<object> getter, Func<object> otherGetter, CancellationToken cancellationToken = new())
        {
            await WaitUntil(() => getter.Invoke() == otherGetter.Invoke(), cancellationToken);
        }
        
        public static async Task WaitUntil(Func<bool> condition, CancellationToken cancellationToken = new())
        {
            while (!condition.Invoke())
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                await Task.Delay(25, cancellationToken);
            }
        }
    }
}