using System;
using System.IO;
using Lazy.IPhoneBackupsManagement.Old;

namespace Lazy.IPhoneBackupsManagement.New.Operations
{
    internal static class FileExtensions
    {
        internal static void Move(this FileInfo fileInfo, string outputDirectory, bool dryRun)
        {
            if (!dryRun)
            {
                var mkDirP = outputDirectory.MkDirP(dryRun);
                var destFileName = CalculateDestinationFileName(mkDirP, fileInfo);
                
                File.Move(fileInfo.FullName,
                    destFileName);
            }
        }

        private static string CalculateDestinationFileName(string directory, FileInfo fileInfo)
        {
            var @base = Path.Combine(directory, fileInfo.Name);
            if (!File.Exists(@base))
                return @base;

            int i = 1;
            while(true)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
                var copy = Path.Combine(directory, $"{fileNameWithoutExtension}-{i}{fileInfo.Extension}");
                if (!File.Exists(copy))
                    return copy;
                i++;
            } 
        }
    }
}