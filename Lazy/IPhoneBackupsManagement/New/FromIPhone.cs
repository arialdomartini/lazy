using System;
using System.IO;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;
using Lazy.IPhoneBackupsManagement.New.Operations;
using NExifTool;

namespace Lazy.IPhoneBackupsManagement.New
{
    public class FromIPhone
    {
        private readonly ExifTool _exifTool;

        internal FromIPhone(ExifTool exifTool)
        {
            _exifTool = exifTool;
        }

        internal void Run(DirectoryInfo workingDirectory, DirectoryInfo output, bool dryRun)
        {
            var results = workingDirectory
                .AllFiles()
                .Select(ToImage)
                .Select((Either<ImageWithoutExifDate, ImageWithExifDate> image) => ToOperation(image, output))
                .Select(operation => operation.Run(dryRun));
            

            Console.WriteLine(string.Join("\n", results));
        }

        private Either<ImageWithoutExifDate, ImageWithExifDate> ToImage(FileInfo fileInfo) =>
            GetDateTimeOriginal(fileInfo).Match(
                dateTime => ImageWithExifDate(fileInfo, dateTime),
                () => ImageWithoutExifDate.Build(fileInfo)
            );

        private static Either<ImageWithoutExifDate, ImageWithExifDate> ImageWithExifDate(FileInfo fileInfo, DateTime dateTime)
        {
            try
            {
                return Right(New.ImageWithExifDate.Build(fileInfo, dateTime));
            }
            catch (Exception)
            {
                Console.WriteLine($"Failed parsing file:\n{fileInfo.FullName}");
                throw;
            }
        }

        private Option<DateTime> GetDateTimeOriginal(FileInfo fileInfo)
        {
            var list = _exifTool.GetTagsAsync(fileInfo.FullName).Result;
            var dateTimeOriginal = list.FirstOrDefault(l => l.Name == "DateTimeOriginal");

            if (dateTimeOriginal is not { IsDate: true })
                return None;

            static DateTime ParseDate(Tag dateTimeOriginal1) =>
                DateTime.Parse(dateTimeOriginal1.Value.Split(" ").First().Replace(":", "/"));

            var dateTime = ParseDate(dateTimeOriginal);
            return Some(dateTime);
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
    }
}