using System;
using System.IO;
using LanguageExt;
using Lazy.IPhoneBackupsManagement.New;

namespace Lazy.ExifManagement.ImageHandlers
{
    internal class RootImageHandler : IImageHandler
    {
        private readonly ExifWrapper _exifWrapper;

        internal RootImageHandler(ExifWrapper exifWrapper)
        {
            _exifWrapper = exifWrapper;
        }

        Option<DateTime> IImageHandler.ReadDateFromExif(FileSystemInfo fileInfo) =>
            _exifWrapper.GetDateTimeOriginal(fileInfo);

        void IImageHandler.UpdateExif(FileInfo fileInfo, DateTime dateTime) =>
            _exifWrapper.UpdateDateTimeOriginal(fileInfo, dateTime);
    }
}