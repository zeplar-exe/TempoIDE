using System;
using System.IO;
using ByteSizeLib;
using Jammo.TextAnalysis.DotNet.MsBuild;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.DataStructures
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
                Content = File.ReadAllText(FileInfo.FullName);
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

    public class CachedProjectFile : CachedFile
    {
        public readonly JProjectFile ProjectFile;

        public CachedProjectFile(FileInfo fileInfo, JProjectFile projectFile) : base(fileInfo)
        {
            ProjectFile = projectFile;
        }
    }
}