using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageExt;
using NExifTool;
using NExifTool.Writer;

namespace Lazy.IPhoneBackupsManagement.New
{
    internal class ExifWrapper
    {
        private const string DatetimeOriginal = "DateTimeOriginal";
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
                .FirstOrDefault(l => l.Name == DatetimeOriginal);

        private static DateTime ParseDate(Tag dateTimeOriginal) =>
            DateTime.Parse(dateTimeOriginal.Value.Split(" ").First().Replace(":", "/"));

        internal void UpdateDateTimeOriginal(FileInfo fileInfo, DateTime dateTime)
        {
            Console.WriteLine($"Updating exif date to {dateTime} to file {fileInfo.FullName}");
            var update = new List<Operation>
            {
                new SetOperation(new Tag(DatetimeOriginal, dateTime.ToString("u"))),
            };

            
            var writeResult = _exifTool.WriteTagsAsync(fileInfo.FullName, update).Result;

            var writeFileStream = new FileStream(fileInfo.FullName, FileMode.Open);
            writeResult.Output.Seek(0, SeekOrigin.Begin);
            writeResult.Output.CopyTo(writeFileStream);
            writeFileStream.Close();
            writeFileStream.Dispose();
        }
    }
}