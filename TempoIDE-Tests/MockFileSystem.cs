using System;
using System.Collections.Generic;
using System.IO;

namespace TempoIDE_Tests
{
    public class MockFileSystem : IDisposable
    {
        public readonly MockDirectory Root;
        
        public MockFileSystem(string name, string directory)
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);

            var path = Path.Join(directory, name);
            
            Directory.CreateDirectory(path);
            Root = new MockDirectory(new DirectoryInfo(path));
        }

        public void CreateDirectory(string relativePath)
        {
            Root.ItemsList.Add(new MockDirectory(Directory.CreateDirectory(Path.Join(Root.Info.FullName, relativePath))));
        }

        public bool DeleteDirectory(MockDirectory directory)
        {
            directory.Dispose();
            
            return Root.ItemsList.Remove(directory);
        }

        public void CreateFile(string relativePath)
        {
            var file = File.Create(Path.Join(Root.Info.FullName, relativePath));
            Root.ItemsList.Add(new MockFile(file));
        }

        public bool DeleteFile(MockFile file)
        {
            file.Dispose();
            
            return Root.ItemsList.Remove(file);
        }
        
        public void Dispose()
        {
            Root.Dispose();   
        }
    }
    
    public abstract class MockFileSystemItem : IDisposable
    {
        public readonly FileSystemInfo Info;

        internal MockFileSystemItem(FileSystemInfo info)
        {
            Info = info;
        }

        public abstract void Dispose();
    }

    public class MockDirectory : MockFileSystemItem
    {
        internal readonly List<MockFileSystemItem> ItemsList = new();
        public IEnumerable<MockFileSystemItem> Items => ItemsList;
        
        internal MockDirectory(DirectoryInfo directory) : base(directory)
        {
            
        }

        public void CreateDirectory(string relativePath)
        {
            ItemsList.Add(new MockDirectory(Directory.CreateDirectory(Path.Join(Info.FullName, relativePath))));
        }

        public bool DeleteDirectory(MockDirectory directory)
        {
            return ItemsList.Remove(directory);
        }

        public void CreateFile(string relativePath)
        {
            var file = File.Create(Path.Join(Info.FullName, relativePath));
            ItemsList.Add(new MockFile(file));
        }

        public bool DeleteFile(MockFile file)
        {
            return ItemsList.Remove(file);
        }

        public override void Dispose()
        {
            foreach (var item in ItemsList)
                item.Dispose();
            
            Directory.Delete(Info.FullName, true);
        }
    }

    public class MockFile : MockFileSystemItem
    {
        private readonly FileStream stream;
        
        internal MockFile(FileStream file) : base(new FileInfo(file.Name))
        {
            stream = file;
        }

        public override void Dispose()
        {
            stream.Dispose();
            
            File.Delete(Info.FullName);
        }
    }
}