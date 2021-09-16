using System;
using System.IO;
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

        public void Update()
        {
            FileInfo.Refresh();

            if (!FileInfo.Exists)
                return;

            try
            {
                using var file = FileInfo.OpenRead();
                using var buffer = new BufferedStream(file);
                using var reader = new StreamReader(buffer);

                Content = reader.ReadToEndAsync().Result;
            }
            catch (AccessViolationException)
            {
                var details = 
                    $"File: {FileInfo.DirectoryName}\\{FileInfo.Name}\n" +
                    $"Security: {FileInfo.GetAccessControl().AccessRightType}";
                    
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_FILE_NO_ACCESS, details);
            }
            catch (OutOfMemoryException)
            {
                var fileSize = ByteSize.FromBits(FileInfo.Length).ToString();

                var details = 
                    $"File: {FileInfo.DirectoryName}\\{FileInfo.Name}\n" +
                    $"File size: {fileSize}";
                    
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_FILE_NOMEM, details);
            }
        }
    }
}