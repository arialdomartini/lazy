using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Lazy.CommandLine;
using Lazy.ExifManagement;
using Lazy.IPhoneBackupsManagement;
using Lazy.IPhoneBackupsManagement.Old;
using CommandExtensions = Lazy.CommandLine.CommandExtensions;

namespace Lazy
{
    static class MainApp
    {
        private const string CurrentPath = ".";

        private static int Main(string[] args)
        {
            var workingDirectory = new DirectoryInfo(CurrentPath);
            
            var fromIPhoneOld = CommandExtensions.Create(
                "from-iphone-old", "Converts a dump from iPhone into a date-based directory structure", 
                dryRun => FromIPhoneOld.Run(workingDirectory, dryRun));

            var fixExif = CommandExtensions.Create(
                "fix-exif", "Set all the missing Exif dates in Exif in a directory, inferring the missing dates from directory names",
                dryRun =>
                {
                    FixExif.Run(workingDirectory, dryRun);
                });
            
            
            var rootCommand = new RootCommand
            {
                new Option("--version", "The current version of this tool"),
                fromIPhoneOld,
                fixExif
            };
            rootCommand.Handler = CommandHandler.Create(Version.VersionHandler);

            return rootCommand.Invoke(args);
        }
    }
}