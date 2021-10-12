using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

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
                MoveFile(dryRun);

            private void MoveFile(bool dryRun)
            {
                Console.Write(
                    $"{_mappableImage.FileInfo.FullName}, UpdateFileSystemDate, tryDelete {_mappableImage.ExifDateAsString} => FileSystem");
                var units = (
                    from exifDate in _mappableImage.ExifDate
                    select MoveTheFile(_mappableImage.FileInfo, dryRun))
                    .ToList();
                
                Console.WriteLine();
            }

            private Unit MoveTheFile(FileInfo fileInfo, bool dryRun)
            {
                var duplicate = fileInfo.Directory
                    .EnumerateFiles("*.*", SearchOption.AllDirectories)
                    .FirstOrDefault(f => f.DirectoryName != fileInfo.DirectoryName && f.Name == fileInfo.Name);

                if (duplicate == null) return unit;
                
                Console.Write($"; Duplicated by {duplicate.FullName}");
                Console.Write($"; rm {fileInfo.FullName}");
                if(!dryRun)
                    File.Delete(fileInfo.FullName);
                return unit;
            }

            private string ParentWithDate(FileInfo fileInfo, in DateTime exifDate) =>
                Path.Combine(fileInfo.Directory.Parent.FullName, exifDate.ToLongDateWithDashes());
        }

    }
}