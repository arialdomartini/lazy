using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Lazy.CommandLine;
using Lazy.ExifManagement;
using Lazy.ExifManagement.ImageHandlers;
using Lazy.IPhoneBackupsManagement.New;
using Lazy.IPhoneBackupsManagement.Old;
using NExifTool;
using CommandExtensions = Lazy.CommandLine.CommandExtensions;

namespace Lazy
{
    static class MainApp
    {
        private const string CurrentPath = ".";

        private static int Main(string[] args)
        {
            var workingDirectory = new DirectoryInfo(CurrentPath);
            var exifTool = new ExifTool(new ExifToolOptions { ExifToolPath = "/usr/bin/vendor_perl/exiftool" });
            var rootImageHandler = new RootImageHandler();
            var exifWrapper = new ExifWrapper(exifTool);

            var fromIPhoneOld = CommandExtensions.Create(
                "from-iphone-old", "Converts a dump from iPhone into a date-based directory structure", 
                dryRun => FromIPhoneOld.Run(workingDirectory, dryRun));

            var fromIPhone = CommandExtensions.CreateWithOutput(
                "from-iphone", "Converts a dump from iPhone into a date-based directory structure",
                (dryRun, output) =>
                {
                    new FromIPhone(exifWrapper)
                        .Run(workingDirectory, output, dryRun);
                    
                });

            var fixExif = CommandExtensions.Create(
                "fix-exif", "Set all the missing Exif dates in Exif in a directory, inferring the missing dates from directory names",
                dryRun =>
                {
                    new FixExif(rootImageHandler).Run(workingDirectory, dryRun);
                });

            var removeDuplicateJpg = CommandExtensions.Create(
                "remove-duplicate-jpg", "Remove the JPG files that duplicate equivalent HEIC images",
                dryRun => RemoveDuplicateJpg.Run(workingDirectory, dryRun)
            );
            
            var rootCommand = new RootCommand
            {
                new Option("--version", "The current version of this tool"),
                fromIPhoneOld,
                fromIPhone,
                fixExif,
                removeDuplicateJpg
            };
            rootCommand.Handler = CommandHandler.Create(Version.VersionHandler);

            return rootCommand.Invoke(args);
        }
    }
}