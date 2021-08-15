using System;
using System.Globalization;
using System.IO;
using ByteSizeLib;
using TempoIDE.Windows;

namespace TempoIDE.Core.Static
{
    public static class ErrorDialogHelper
    {
        public static void FileOutOfMemoryException(FileInfo file)
        {
            EnvironmentHelper.AppDispatcher.Invoke(delegate 
            {
                var fileSize = ByteSize.FromBits(file.Length).GigaBytes.ToString(CultureInfo.CurrentCulture);
                
                ErrorDialog.ShowError($"TempoIDE ran out of memory and this file is too large to process ({fileSize}).");
            });
        }
    }
}