using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazy.ExifManagement.Commands;
using Lazy.ExifManagement.ImageHandlers;

namespace Lazy.ExifManagement
{
    internal static class FixExif
    {
        private static readonly HashSet<string> ImageExtensions;
        private static readonly RootImageHandler RootImageHandler;
        private static readonly CommandBuilder CommandBuilder;

        static FixExif()
        {
            RootImageHandler = new RootImageHandler();
            CommandBuilder = new CommandBuilder(new List<ICondition>
            {
                new BothDefined(RootImageHandler),
                new NothingDefined(),
                new OnlyExifDefined(),
                new OnlyFileSystemDefined(RootImageHandler)
            });
            
            ImageExtensions = new(StringComparer.OrdinalIgnoreCase) {".jpg", ".heic"};
        }

        internal static void Run(DirectoryInfo workingDirectory, bool dryRun)
        {
            workingDirectory
                .GetImages()
                .Select(mappableImage => CommandBuilder.CommandFor(mappableImage))
                .ToList()
                .ForEach(c =>
                {
                    c.Run(dryRun);
                    //Console.WriteLine();
                });
        }

        private static IEnumerable<MappableImage> GetImages(this DirectoryInfo workingDirectory) =>
            AllImagesIn(workingDirectory)
                .Select(ToMappableImage);

        private static IEnumerable<FileInfo> AllImagesIn(DirectoryInfo directory) =>
            directory
                .AllFiles()
                .Where(IsAnImage);

        private static bool IsAnImage(FileInfo fileInfo) =>
            fileInfo.HasOfOfTheExtensions(ImageExtensions);

        private static bool HasOfOfTheExtensions(this FileSystemInfo fileInfo, IReadOnlySet<string> imageExtensions) =>
            imageExtensions.Contains(Path.GetExtension(fileInfo.Name));

        private static MappableImage ToMappableImage(FileInfo fileInfo)
        {
            try
            {
                return MappableImage.From(fileInfo, RootImageHandler);
            }
            catch (Exception)
            {
                Console.WriteLine($"Failure: unable to convert {fileInfo.FullName}");
                throw;
            }
        }
    }
}