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
        
        internal Option<DateTime> GetDateTimeOriginal(FileSystemInfo fileInfo)
        {
            var list = _exifTool.GetTagsAsync(fileInfo.FullName).Result;
            var dateTimeOriginal = list.FirstOrDefault(l => l.Name == "DateTimeOriginal");

            if (dateTimeOriginal is not { IsDate: true })
                return Prelude.None;

            static DateTime ParseDate(Tag dateTimeOriginal1) =>
                DateTime.Parse(dateTimeOriginal1.Value.Split(" ").First().Replace(":", "/"));

            var dateTime = ParseDate(dateTimeOriginal);
            return Prelude.Some(dateTime);
        }

    }
}