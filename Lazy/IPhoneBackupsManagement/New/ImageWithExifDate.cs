using System;
using System.IO;
using System.Linq;
using NExifTool;

namespace Lazy.IPhoneBackupsManagement.New
{
    internal class ImageWithExifDate
    {
        internal FileInfo FileInfo { get; init; }
        internal DateTime DateTimeOriginal { get; init; }

        internal static ImageWithExifDate Build(FileInfo fileInfo, Tag? dateTimeOriginal) =>
            new()
            {
                FileInfo = fileInfo, 
                DateTimeOriginal = ParseDate(dateTimeOriginal)
            };

        private static DateTime ParseDate(Tag? dateTimeOriginal) =>
            DateTime.Parse(dateTimeOriginal.Value.Split(" ").First().Replace(":", "/"));
    }
}