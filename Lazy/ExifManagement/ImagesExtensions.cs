using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazy.ExifManagement.ImageHandlers;

namespace Lazy.ExifManagement
{
    internal static class ImagesExtensions
    {
        internal static IEnumerable<MappableImage> GetImages(this DirectoryInfo workingDirectory,
            RootImageHandler rootImageHandler) =>
            workingDirectory
                .AllFiles()
                .Select(fileInfo => ToMappableImage(fileInfo, rootImageHandler));

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