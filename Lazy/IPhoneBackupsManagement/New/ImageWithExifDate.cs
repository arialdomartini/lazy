using System;
using System.IO;

namespace Lazy.IPhoneBackupsManagement.New
{
    internal class ImageWithExifDate
    {
        internal FileInfo FileInfo { get; init; }
        internal DateTime DateTimeOriginal { get; init; }

        internal static ImageWithExifDate Build(FileInfo fileInfo, DateTime dateTime) =>
            new()
            {
                FileInfo = fileInfo,
                DateTimeOriginal = dateTime
            };
    }
}