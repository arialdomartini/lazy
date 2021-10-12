using System;
using System.IO;
using Lazy.ExifManagement.ImageHandlers;

namespace Lazy.ExifManagement.Commands
{
    internal class BothDefined : ICondition
    {
        private readonly RootImageHandler _rootImageHandler;

        public BothDefined(RootImageHandler rootImageHandler)
        {
            _rootImageHandler = rootImageHandler;
        }

        bool ICondition.CanHandle(MappableImage mappableImage) =>
            mappableImage.ExifDate.IsSome &&
            mappableImage.FileSystemDate.IsSome;

        ICommand ICondition.GetCommand(MappableImage mappableImage) =>
            new AssertExifAndFileSystemDatesMatch(mappableImage, _rootImageHandler);
        
        private class AssertExifAndFileSystemDatesMatch : ICommand
        {
            private readonly MappableImage _mappableImage;
            private readonly IImageHandler _rootImageHandler;

            public AssertExifAndFileSystemDatesMatch(MappableImage mappableImage, IImageHandler rootImageHandler)
            {
                _mappableImage = mappableImage;
                _rootImageHandler = rootImageHandler;
            }

            void ICommand.Run(bool dryRun)
            {
                var fileSystemDate = _mappableImage.FileSystemDate;
                var exifDate = _rootImageHandler.ReadDateFromExif(_mappableImage.FileInfo);
                
                var areEqual = 
                    from fileSystem in fileSystemDate 
                    from exif in exifDate
                    select fileSystem.Date == exif.Date;

                var message = areEqual.Match(
                    areEqual => areEqual ? "OK" : $"Failure: Dates do not match (Exif: {exifDate}, FileSystem: {fileSystemDate})",
                    () => throw new InvalidDataException());
                Console.WriteLine($"{_mappableImage.FileInfo.FullName}, AssertExifAndFileSystemDatesMatch, {message}");
            }
        }
    }
}