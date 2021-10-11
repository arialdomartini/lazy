using System;
using System.IO;
using System.Linq;
using LanguageExt;
using Lazy.ImageHandlers;
using static LanguageExt.Prelude;

namespace Lazy
{
    internal static class OptionExtensions
    {
        internal static string Show(this Option<DateTime> dateTime) =>
            dateTime.Match(d => d.ToString("yyyy-MM-dd"), () => "not defined");
    }

    internal class MappableImage
    {
        internal string ImageName { get; init; }
        internal string ContainingDirectory { get; init; }
        internal FileInfo FileInfo { get; init; }
        internal Option<DateTime> FileSystemDate { get; init; }
        internal string FileSystemDateAsString => FileSystemDate.Show();
        internal string ExifDateAsString => ExifDate.Show();
        internal Option<DateTime> ExifDate { get; init; }

    internal string Show() =>
$@"Image Name:            {ImageName};
Containing Directory:  {ContainingDirectory}; 
FileSystem Date:       {FileSystemDateAsString}
Exif Date:             {ExifDateAsString}";

        internal static MappableImage From(FileInfo fileInfo, RootImageHandler rootImageHandler) =>
            new()
            {
                FileInfo = fileInfo,
                ImageName = fileInfo.Name,
                ContainingDirectory = LastContainingDirectory(fileInfo),
                FileSystemDate = InferDateFromFileSystem(fileInfo),
                ExifDate = ((IImageHandler) rootImageHandler).ReadDateFromExif(fileInfo)
            };

        private static Option<DateTime> InferDateFromFileSystem(FileInfo fileInfo)
        {
            var containingDirectory = LastContainingDirectory(fileInfo);
            var maybeDate = containingDirectory.Split(" ").First();

            if(DateTime.TryParse(maybeDate, out var date))
                return date;
                
            return None;
        }

        private static string LastContainingDirectory(FileInfo fileInfo) =>
            Directory.GetParent(fileInfo.FullName).Name.TrimEnd(Path.DirectorySeparatorChar);
    }
}