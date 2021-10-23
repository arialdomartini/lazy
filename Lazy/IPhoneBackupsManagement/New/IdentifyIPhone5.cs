using System;
using System.IO;
using System.Linq;

namespace Lazy.IPhoneBackupsManagement.New
{
    public class IdentifyIPhone5
    {
        private readonly ExifWrapper _exifWrapper;

        internal IdentifyIPhone5(ExifWrapper exifWrapper)
        {
            _exifWrapper = exifWrapper;
        }

        internal void Run(DirectoryInfo workingDirectory, bool dryRun)
        {
            var results = workingDirectory
                .AllFiles()
                .Select(f => (File: f.FullName, Model: TookWithIPhone5(f)))
                .Where(tuple => tuple.Model == "iPhone 5s")
                .Select(tuple => tuple.File);

            Console.WriteLine(string.Join("\n", results));
        }

        private string TookWithIPhone5(FileInfo fileInfo) =>
            _exifWrapper.GetLensId(fileInfo);
    }
}