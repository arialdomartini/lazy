using System;
using System.IO;
using LanguageExt;

namespace Lazy.ExifManagement
{
    internal interface IImageHandler
    {
        Option<DateTime> ReadDateFromExif(FileSystemInfo fileInfo);
        bool CanHandle(FileSystemInfo fileInfo);
        void UpdateExif(FileInfo fileInfo, DateTime dateTime);
    }
}