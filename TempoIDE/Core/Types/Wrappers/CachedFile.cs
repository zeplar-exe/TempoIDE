using System;
using System.IO;
using System.Text;
using ByteSizeLib;
using TempoIDE.Core.Static;

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
                await using var file = FileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                await using var buffer = new BufferedStream(file);
                using var reader = new StreamReader(buffer);

                Content = await reader.ReadToEndAsync();
            }
            catch (AccessViolationException)
            {
                // TODO: Dialog
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