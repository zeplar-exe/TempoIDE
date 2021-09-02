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
                await using var file = FileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                await using var buffer = new BufferedStream(file);
                using var reader = new StreamReader(buffer);

                Content = await reader.ReadToEndAsync();
            }
            catch (AccessViolationException)
            {
                var details = 
                    $"File: {FileInfo.DirectoryName}\\{FileInfo.Name}" +
                    $"Security: {FileInfo.GetAccessControl().AccessRightType}";
                    
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_FILE_NO_ACCESS, details);
            }
            catch (OutOfMemoryException)
            {
                var fileSize = ByteSize.FromBits(FileInfo.Length).ToString();

                var details = 
                    $"File: {FileInfo.DirectoryName}\\{FileInfo.Name}" +
                    $"File size: {fileSize}";
                    
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_FILE_NOMEM, details);
            }
        }
    }
}