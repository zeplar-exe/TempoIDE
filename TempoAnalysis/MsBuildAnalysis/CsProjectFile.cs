using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TempoAnalysis.MsBuildAnalysis
{
    public class CsProjectFile
    {
        private const string RootName = "Project";
        private const string ItemGroupName = "ItemGroup";
        private const string PropertyGroupName = "PropertyGroup";

        public readonly string FilePath;
        public IEnumerable<FileSystemInfo> Files;
        public readonly XDocument ProjectHead;
        
        public IEnumerable<XElement> ItemGroups => ProjectHead.Root
            .Elements()
            .Where(e => e.Name == ItemGroupName);

        public IEnumerable<XElement> PropertyGroups => ProjectHead.Root
            .Elements()
            .Where(e => e.Name == PropertyGroupName);
        
        public CsProjectFile(string path)
        {
            var info = new FileInfo(path);
            
            if (!info.Exists)
                throw new ArgumentException($"File '{info.Name}' does not exist.");

            if (info.Extension != ".csproj")
                throw new ArgumentException("Expected a .csproj file.");
            
            if (info.Directory == null)
                return;

            var reader = new StreamReader(info.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            var document = XDocument.Parse(reader.ReadToEndAsync().Result);

            var root = document.Root;
            
            if (root?.Name != RootName)
                return;

            FilePath = path;
            ProjectHead = document;

            var filePathAttributes = new[] { "Include", "Update", "Remove" };
            
            var specialItems = ItemGroups
                .Select(g => g
                    .Elements()
                    .Where(e => filePathAttributes.Contains(e.Attributes().FirstOrDefault()?.Value.ToString())))
                .SelectMany(e => e)
                .ToArray();

            var system = new ProjectFileSystem(ReadDirectory(info.Directory));

            foreach (var item in specialItems)
            {
                var removeAttribute = item.Attributes().First().Name == "Remove" ? item.Attributes().First() : null;

                if (removeAttribute != null)
                {
                    system.TopLevel.TraverseRelativePath(removeAttribute.Value);
                } // TODO: Come back to this crap
            }
        }

        private ProjectDirectory ReadDirectory(DirectoryInfo info)
        {
            var dir = new ProjectDirectory(info);
            
            foreach (var entry in info.EnumerateFileSystemInfos())
            {
                if (entry is FileInfo file)
                    dir.Children.Add(new ProjectFile(file));
                else if (entry is DirectoryInfo directory)
                    dir.Children.Add(ReadDirectory(directory));
            }

            return dir;
        }
    }

    public class ProjectFileSystem
    {
        public readonly ProjectDirectory TopLevel;

        public ProjectFileSystem(ProjectDirectory directory)
        {
            TopLevel = directory;
        }
    }

    public interface IProjectFileSystemEntry
    {
        public FileSystemInfo Info { get; }
    }

    public class ProjectDirectory : IProjectFileSystemEntry
    {
        public FileSystemInfo Info { get; private set; }
        public readonly List<IProjectFileSystemEntry> Children = new();

        internal ProjectDirectory(DirectoryInfo info)
        {
            Info = info;
        }

        public IProjectFileSystemEntry TraverseRelativePath(string path)
        {
            path = path.Replace("\\", "/");
            
            return TraverseRelativePath(path.Split("/"));
        }

        private IProjectFileSystemEntry TraverseRelativePath(string[] path)
        {
            var current = path.FirstOrDefault();

            if (current is null)
                return null;
            
            foreach (var entry in Children)
            {
                if (current.StartsWith(entry.Info.Name))
                {
                    if (current == entry.Info.Name)
                        return entry;

                    if (entry is ProjectDirectory projectDirectory)
                    {
                        return projectDirectory.TraverseRelativePath(path.Skip(1).ToArray());
                    }
                }
            }

            return null;
        }
    }

    public class ProjectFile : IProjectFileSystemEntry
    {
        public FileSystemInfo Info { get; private set; }
        public readonly List<IProjectFileSystemEntry> DependantFiles = new();

        internal ProjectFile(FileInfo info)
        {
            Info = info;
        }
    }
}