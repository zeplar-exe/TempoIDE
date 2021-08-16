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
                var fileSize = ByteSize.FromBits(file.Length).ToString();
                var message = $"TempoIDE ran out of memory trying to process this file ({fileSize}).";
                
                var dialog = new UserDialog(message, UserResult.Ok);

                dialog.ShowDialog();
            });
        }
    }
}