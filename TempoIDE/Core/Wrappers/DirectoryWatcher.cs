using System;
using System.Collections.Generic;
using System.IO;

namespace TempoIDE.Core.Wrappers
{
    public class DirectoryWatcher : IDisposable
    {
        private bool buffering;
        private readonly Queue<(object sender, FileSystemEventArgs args)> eventBuffer = new();
        private readonly FileSystemWatcher watcher;
        
        public event FileSystemEventHandler Changed;

        public DirectoryWatcher(DirectoryInfo directory, string filter = "*")
        {
            watcher = new FileSystemWatcher(directory.FullName);

            watcher.NotifyFilter = NotifyFilters.Attributes |
                                   NotifyFilters.Security |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.FileName |
                                   NotifyFilters.LastWrite | 
                                   NotifyFilters.CreationTime;
            
            watcher.Changed += InvokeEvent;
            watcher.Created += InvokeEvent;
            watcher.Deleted += InvokeEvent;
            watcher.Renamed += InvokeEvent;

            watcher.Filter = filter;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        private void InvokeEvent(object sender, FileSystemEventArgs e)
        {
            if (buffering)
            {
                eventBuffer.Enqueue((sender, e));
                
                return;
            }
            
            Changed?.Invoke(sender, e);
        }

        public void Buffer()
        {
            buffering = true;
        }

        public void Resume()
        {
            buffering = false;
            
            while (eventBuffer.Count > 0)
            {
                var (sender, args) = eventBuffer.Dequeue();

                Changed?.Invoke(sender, args);
            }
        }

        public void Dispose()
        {
            watcher.Dispose();
            Changed = null;
        }
    }
}