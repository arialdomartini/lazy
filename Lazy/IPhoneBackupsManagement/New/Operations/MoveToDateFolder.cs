using System;
using System.IO;

namespace Lazy.IPhoneBackupsManagement.New.Operations
{
    internal class MoveToDateFolder : IOperation
    {
        private readonly ImageWithExifDate _image;
        private readonly DirectoryInfo _outputDirectory;

        internal MoveToDateFolder(ImageWithExifDate image, DirectoryInfo outputDirectory)
        {
            _image = image;
            _outputDirectory = outputDirectory;
        }

        string IOperation.Run(bool dryRun)
        {
            var imageDateTimeOriginal = _image.DateTimeOriginal;
            var outputDirectory =
                Path.Combine(
                    _outputDirectory.FullName,
                    imageDateTimeOriginal.Year.ToString("0000"),
                    imageDateTimeOriginal.Month.ToString("00"),
                    imageDateTimeOriginal.Day.ToString("00")
                    );

            _image.FileInfo.Move(outputDirectory, dryRun);
            return $"mv {_image.FileInfo.FullName} {outputDirectory}";
        }
    }
}