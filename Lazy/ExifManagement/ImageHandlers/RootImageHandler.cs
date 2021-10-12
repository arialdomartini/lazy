using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageExt;

namespace Lazy.ExifManagement.ImageHandlers
{
    internal class RootImageHandler : IImageHandler
    {
        private readonly List<IImageHandler> _imageHandlers;

        internal RootImageHandler()
        {
            _imageHandlers = new()
            {
                new JpegImageHandler(),
                new HeifImageHandler()
            };
        }

        private IImageHandler For(FileSystemInfo fileInfo) =>
            _imageHandlers.First(i => i.CanHandle(fileInfo));

        Option<DateTime> IImageHandler.ReadDateFromExif(FileSystemInfo fileInfo) =>
            For(fileInfo).ReadDateFromExif(fileInfo);

        bool IImageHandler.CanHandle(FileSystemInfo fileInfo) =>
            true;

        void IImageHandler.UpdateExif(FileInfo fileInfo, DateTime dateTime)
        {
            For(fileInfo).UpdateExif(fileInfo, dateTime);
        }
    }
}