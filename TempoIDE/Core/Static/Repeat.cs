using System;
using System.Threading;
using System.Threading.Tasks;

namespace TempoIDE.Core.Static
{
    // Courtesy of https://stackoverflow.com/a/7472334/16324801
    
    internal static class Repeat
    {
        public static Task Interval(
            TimeSpan pollInterval,
            Action action,
            CancellationToken token)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        if (token.WaitCancellationRequested(pollInterval))
                            break;

                        action();
                    }
                }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }

    static class CancellationTokenExtensions
    {
        public static bool WaitCancellationRequested(
            this CancellationToken token,
            TimeSpan timeout)
        {
            return token.WaitHandle.WaitOne(timeout);
        }
    }
}