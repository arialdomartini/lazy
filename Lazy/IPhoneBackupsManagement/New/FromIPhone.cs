using System;
using System.IO;
using System.Linq;
using LanguageExt;
using Lazy.IPhoneBackupsManagement.New.Operations;
using NExifTool;

namespace Lazy.IPhoneBackupsManagement.New
{
    public class FromIPhone
    {
        private readonly ExifTool _exifTool;

        internal FromIPhone()
        {
            _exifTool = new ExifTool(new ExifToolOptions {ExifToolPath = "/usr/bin/vendor_perl/exiftool"});
        }

        internal void Run(DirectoryInfo workingDirectory, DirectoryInfo output, bool dryRun)
        {
            var results = workingDirectory
                .AllFiles()
                .Select(ToImage)
                .Select((Either<ImageWithoutExifDate, ImageWithExifDate> image) => ToOperation(image, output))
                .Select(operation => operation.Run());
            
            Console.WriteLine(string.Join("\n", results));
        }

        private Either<ImageWithoutExifDate, ImageWithExifDate> ToImage(FileInfo fileInfo)
        {
            var list = _exifTool.GetTagsAsync(fileInfo.FullName).Result;
            var dateTimeOriginal = list.FirstOrDefault(l => l.Name == "DateTimeOriginal");

            if (dateTimeOriginal == null)
                return ImageWithoutExifDate.Build(fileInfo);
            
            return ImageWithExifDate.Build(fileInfo, dateTimeOriginal);
        }

        private static IOperation ToOperation(Either<ImageWithoutExifDate, ImageWithExifDate> image,
            DirectoryInfo outputDirectory) =>
            image.Match<IOperation>(
                date => MoveToDateFolder(date, outputDirectory),
                date => MoveToCatchallFolder(date, outputDirectory));

        private static MoveToCatchallFolder MoveToCatchallFolder(ImageWithoutExifDate image,
            DirectoryInfo outputDirectory) =>
            new(image, outputDirectory);

        private static MoveToDateFolder MoveToDateFolder(ImageWithExifDate image, DirectoryInfo outputDirectory) =>
            new(image, outputDirectory);

        private static void PrintDateTimeOriginal(ExifTool et, FileInfo fileInfo)
        {
            var list = et.GetTagsAsync(fileInfo.FullName).Result;
            var firstOrDefault = list.FirstOrDefault(l => l.Name == "DateTimeOriginal");
            Option<string> dateTimeOriginal = firstOrDefault?.Value;

            if (dateTimeOriginal.IsSome)
                Console.WriteLine($"File: {fileInfo.FullName}; ExifTags: {string.Join(",", dateTimeOriginal)}.");
            else
            {
                Console.WriteLine($"File: {fileInfo.FullName}; No exif date is defined");
            }
        }
    }
}