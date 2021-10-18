using System.IO;

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

        string IOperation.Run()
        {
            var output = Path.Combine(_outputDirectory.FullName, "_noDate");
            return $"mv {_image.FileInfo.FullName} {output}";
        }
    }
}