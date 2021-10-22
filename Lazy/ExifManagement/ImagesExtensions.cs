using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazy.ExifManagement.ImageHandlers;

namespace Lazy.ExifManagement
{
    internal static class ImagesExtensions
    {
        private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase) {".jpg", ".heic"};

        internal static IEnumerable<MappableImage> GetImages(this DirectoryInfo workingDirectory, RootImageHandler rootImageHandler) =>
            AllImagesIn(workingDirectory)
                .Select(fileInfo => ToMappableImage(fileInfo, rootImageHandler));

        private static IEnumerable<FileInfo> AllImagesIn(DirectoryInfo directory) =>
            directory
                .AllFiles()
                .Where(IsAnImage);

        private static bool IsAnImage(FileInfo fileInfo) =>
            fileInfo.HasOfOfTheExtensions(ImageExtensions);

        private static bool HasOfOfTheExtensions(this FileSystemInfo fileInfo, IReadOnlySet<string> imageExtensions) =>
            imageExtensions.Contains(Path.GetExtension(fileInfo.Name));

        private static MappableImage ToMappableImage(FileInfo fileInfo, RootImageHandler rootImageHandler)
        {
            try
            {
                return MappableImage.From(fileInfo, rootImageHandler);
            }
            catch (Exception)
            {
                Console.WriteLine($"Failure: unable to convert {fileInfo.FullName}");
                throw;
            }
        }
    }
}