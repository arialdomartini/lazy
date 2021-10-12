using System;
using System.Data;
using Lazy.ImageHandlers;

namespace Lazy.Commands
{
    internal class OnlyFileSystemDefined : ICondition
    {
        private readonly RootImageHandler _rootImageHandler;

        public OnlyFileSystemDefined(RootImageHandler rootImageHandler)
        {
            _rootImageHandler = rootImageHandler;
        }

        bool ICondition.CanHandle(MappableImage mappableImage) =>
            mappableImage.ExifDate.IsNone && 
            mappableImage.FileSystemDate.IsSome;

        ICommand ICondition.GetCommand(MappableImage mappableImage) =>
            new UpdateExifDate(mappableImage, _rootImageHandler);
        
        private class UpdateExifDate : ICommand
        {
            private readonly MappableImage _mappableImage;
            private readonly IImageHandler _rootImageHandler;

            internal UpdateExifDate(MappableImage mappableImage, RootImageHandler rootImageHandler)
            {
                _mappableImage = mappableImage;
                _rootImageHandler = rootImageHandler;
            }

            void ICommand.Run(bool dryRun)
            {
                Console.WriteLine(
                    $"{_mappableImage.FileInfo.FullName}, UpdateExifDate, {_mappableImage.FileSystemDateAsString} => Exif");

                _mappableImage.FileSystemDate.Match(
                    date => UpdateExif(date, dryRun),
                    () => throw new InvalidConstraintException());
            }

            private void UpdateExif(DateTime date, bool dryRun)
            {
                if(!dryRun)
                    _rootImageHandler.UpdateExif(_mappableImage.FileInfo, date);
            }
        }

    }
}