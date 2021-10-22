using System;
using System.IO;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;
using Lazy.IPhoneBackupsManagement.New.Operations;

namespace Lazy.IPhoneBackupsManagement.New
{
    public class FromIPhone
    {
        private readonly ExifWrapper _exifWrapper;

        internal FromIPhone(ExifWrapper exifWrapper)
        {
            _exifWrapper = exifWrapper;
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
            _exifWrapper.GetDateTimeOriginal(fileInfo).Match(
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