using System.IO;

namespace Lazy.IPhoneBackupsManagement.New
{
    internal class ImageWithoutExifDate
    {
        internal FileInfo FileInfo { get; init; }

        internal static ImageWithoutExifDate Build(FileInfo fileInfo) =>
            new()
            {
                FileInfo = fileInfo
            };
    }
}