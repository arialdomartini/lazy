using System;
using System.IO;
using System.Linq;
using LanguageExt;
using NExifTool;

namespace Lazy.IPhoneBackupsManagement.New
{
    internal class ExifWrapper
    {
        private readonly ExifTool _exifTool;

        internal ExifWrapper(ExifTool exifTool)
        {
            _exifTool = exifTool;
        }
        
        internal Option<DateTime> GetDateTimeOriginal(FileSystemInfo fileInfo) =>
            TryParse(ReadTagFromExif(fileInfo));

        private static Option<DateTime> TryParse(Tag? dateTimeOriginal) =>
            dateTimeOriginal is { IsDate: true } ? 
                Prelude.Some(ParseDate(dateTimeOriginal)) : 
                Prelude.None;

        private Tag? ReadTagFromExif(FileSystemInfo fileInfo) =>
            _exifTool
                .GetTagsAsync(fileInfo.FullName).Result
                .FirstOrDefault(l => l.Name == "DateTimeOriginal");

        private static DateTime ParseDate(Tag dateTimeOriginal) =>
            DateTime.Parse(dateTimeOriginal.Value.Split(" ").First().Replace(":", "/"));
    }
}