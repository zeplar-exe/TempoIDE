using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jammo.ParserTools;

namespace TempoIDE.Plugins.Core;

public class PluginStream : IParserStream
{
    private FileStream stream;

    public bool IsInitialized => stream == null;
    public string FilePath => stream.Name;
        
    public string Version = "";
    public readonly Dictionary<string, string> Metadata = new(StringComparer.InvariantCulture);

    public string PluginName
    {
        get
        {
            Metadata.TryGetValue("name", out var name);

            return name;
        }
    }

    public bool TryGetDirectoryPath(out string result)
    {
        result = null;
            
        Metadata.TryGetValue("directory", out var directoryPath);

        if (directoryPath == null)
        {
            if (PluginName == null)
                return false;

            var info = new FileInfo(FilePath);

            directoryPath = Path.Join(info.DirectoryName, PluginName);
        }

        if (!Directory.Exists(directoryPath))
            return false;

        result = directoryPath;

        return true;
    }

    public PluginStream(FileStream stream = null)
    {
        this.stream = stream;
    }

    public void Parse()
    {
        using var reader = new StreamReader(stream);
        using var newStream = PluginParser.Parse(reader.ReadToEndAsync().Result);

        Version = newStream.Version;
        Metadata.Clear();
            
        foreach (var (key, value) in newStream.Metadata)
            Metadata.Add(key, value);
    }

    public void Write()
    {
        if (stream == null)
        {
            var working = Directory.GetCurrentDirectory();

            Console.WriteLine("The current stream is null, a new file will be created in the working directory.\n" +
                              $"Current working directory: {working}");
                
            stream = File.Create(Path.Join(Directory.GetCurrentDirectory(), "Jammo_SolutionStream.sln"));
        }
            
        var builder = new StringBuilder();
            
        builder.AppendLine(Version);
        builder.AppendLine();
        builder.AppendLine(Metadata.ToString());
            
        File.WriteAllText(FilePath, builder.ToString());
    }

    public void WriteTo(string path)
    {
        using var file = File.Create(path);
        using var writer = new StreamWriter(file);
            
        file.SetLength(0);
        writer.Write(ToString());
    }

    public void Dispose()
    {
        stream?.Dispose();
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.AppendLine(Version);
        builder.AppendLine();
        builder.AppendLine(Metadata.ToString());

        return builder.ToString();
    }
}