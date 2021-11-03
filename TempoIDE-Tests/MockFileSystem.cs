using System;
using System.Collections.Generic;
using System.IO;

namespace TempoIDE_Tests
{
    public class MockFileSystem : IDisposable
    {
        public readonly MockDirectory Root;
        
        public MockFileSystem(string name, DirectoryInfo directory)
        {
            var path = Path.Join(directory.FullName, name);
            var directoryInfo = new DirectoryInfo(path);
            
            if (Directory.Exists(directoryInfo.FullName))
                Directory.Delete(directoryInfo.FullName, true);
            
            Directory.CreateDirectory(path);
            Root = new MockDirectory(new DirectoryInfo(path));
        }

        public void Dispose()
        {
            Root.Dispose();   
        }
    }
    
    public abstract class MockFileSystemItem : IDisposable
    {
        protected readonly FileSystemInfo info;

        internal MockFileSystemItem(FileSystemInfo info)
        {
            this.info = info;
        }

        public abstract void Dispose();
    }

    public class MockDirectory : MockFileSystemItem
    {
        public DirectoryInfo Info => (DirectoryInfo)info;
        
        private readonly List<MockFileSystemItem> items = new();
        public IEnumerable<MockFileSystemItem> Items => items.AsReadOnly();
        
        internal MockDirectory(DirectoryInfo directory) : base(directory)
        {
            
        }

        public MockDirectory CreateDirectory(string relativePath)
        {
            var directory = new MockDirectory(Directory.CreateDirectory(Path.Join(Info.FullName, relativePath)));
            
            items.Add(directory);

            return directory;
        }

        public bool DeleteDirectory(MockDirectory directory)
        {
            return items.Remove(directory);
        }

        public MockFile CreateFile(string relativePath)
        {
            var file = new MockFile(new FileInfo(Path.Join(Info.FullName, relativePath)));
            
            items.Add(file);

            return file;
        }

        public bool DeleteFile(MockFile file)
        {
            return items.Remove(file);
        }

        public override void Dispose()
        {
            foreach (var item in items)
                item.Dispose();
            
            Directory.Delete(Info.FullName, true);
        }
    }

    public class MockFile : MockFileSystemItem
    {
        public FileInfo Info => (FileInfo)info;
        
        internal MockFile(FileInfo file) : base(file)
        {
            
        }

        public StreamReader ReadStream() => new(Info.OpenRead());
        public StreamWriter WriteToStream() => new(Info.OpenWrite());

        public override void Dispose()
        {
            File.Delete(Info.FullName);
        }
    }
}