using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteSizeLib;
using TempoIDE.Core.Static;
using TempoIDE.Windows;

namespace TempoIDE.Core.Types.Wrappers
{
    public class CachedFile
    {
        public readonly FileInfo FileInfo;
        public string Content { get; private set; }

        public CachedFile(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
            
            Update();
        }

        public async void Update()
        {
            FileInfo.Refresh();

            if (!FileInfo.Exists)
                return;

            try
            {
                await using var file = FileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                await using var buffer = new BufferedStream(file);
                using var reader = new BinaryReader(buffer);

                var bom = new byte[4];
                reader.Read(bom, 0, 4);

                await Task.Run(delegate
                {
                    Content = Encoding.UTF8.GetString(bom.Concat(reader.ReadAllBytes()).ToArray());
                });
            }
            catch (OutOfMemoryException)
            {
                var fileSize = ByteSize.FromBits(FileInfo.Length).ToString();
                    
                var details = new StringBuilder();
                details.AppendLine($@"File: {FileInfo.DirectoryName}\{FileInfo.Name}");
                details.AppendLine($"File size: {fileSize}");
                    
                ApplicationHelper.ThrowErrorCode(ApplicationErrorCode.TI_FILE_NOMEM, details.ToString());
                throw;
            }
        }
    }
}