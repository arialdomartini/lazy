using System;

namespace Lazy.Commands
{
    internal class OnlyExifDefined : ICondition
    {
        bool ICondition.CanHandle(MappableImage mappableImage) =>
            mappableImage.ExifDate.IsSome && 
            mappableImage.FileSystemDate.IsNone;

        ICommand ICondition.GetCommand(MappableImage mappableImage) =>
            new UpdateFileSystemDate(mappableImage);
        
        private class UpdateFileSystemDate : ICommand
        {
            private readonly MappableImage _mappableImage;

            internal UpdateFileSystemDate(MappableImage mappableImage)
            {
                _mappableImage = mappableImage;
            }

            void ICommand.Run(bool dryRun) =>
                Console.WriteLine($"{_mappableImage.FileInfo.FullName}, UpdateFileSystemDate, NOOP {_mappableImage.ExifDateAsString} => FileSystem");
        }

    }
}