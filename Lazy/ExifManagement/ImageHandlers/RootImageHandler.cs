using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageExt;
using Lazy.IPhoneBackupsManagement.New;

namespace Lazy.ExifManagement.ImageHandlers
{
    internal class RootImageHandler : IImageHandler
    {
        private readonly ExifWrapper _exifWrapper;
        private readonly List<IImageHandler> _imageHandlers;

        internal RootImageHandler(ExifWrapper exifWrapper)
        {
            _exifWrapper = exifWrapper;
            _imageHandlers = new()
            {
                new JpegImageHandler(),
                new HeifImageHandler()
            };
        }

        private IImageHandler For(FileSystemInfo fileInfo) =>
            _imageHandlers.First(i => i.CanHandle(fileInfo));

        Option<DateTime> IImageHandler.ReadDateFromExif(FileSystemInfo fileInfo) =>
            _exifWrapper.GetDateTimeOriginal(fileInfo);

        bool IImageHandler.CanHandle(FileSystemInfo fileInfo) =>
            true;

        void IImageHandler.UpdateExif(FileInfo fileInfo, DateTime dateTime) =>
            _exifWrapper.UpdateDateTimeOriginal(fileInfo, dateTime);
    }
}