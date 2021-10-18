using System.IO;
using Lazy.IPhoneBackupsManagement.Old;

namespace Lazy.IPhoneBackupsManagement.New.Operations
{
    internal class MoveToCatchallFolder : IOperation
    {
        private readonly ImageWithoutExifDate _image;
        private readonly DirectoryInfo _outputDirectory;

        internal MoveToCatchallFolder(ImageWithoutExifDate image, DirectoryInfo outputDirectory)
        {
            _image = image;
            _outputDirectory = outputDirectory;
        }

        string IOperation.Run(bool dryRun)
        {
            var outputDirectory = Path.Combine(_outputDirectory.FullName, "_noDate");
            
            _image.FileInfo.Move(outputDirectory, dryRun);
            
            return $"mv {_image.FileInfo.FullName} {outputDirectory.MkDirP(dryRun)}";
        }
    }
}