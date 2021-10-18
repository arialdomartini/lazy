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
                File.Move(fileInfo.FullName,
                    Path.Combine(outputDirectory.MkDirP(dryRun), fileInfo.Name));
            }
        }
    }
}