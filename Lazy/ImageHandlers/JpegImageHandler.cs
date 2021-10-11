using System;
using System.IO;
using ExifLibrary;
using LanguageExt;

namespace Lazy.ImageHandlers
{
    internal class JpegImageHandler : IImageHandler
    {
        Option<DateTime> IImageHandler.ReadDateFromExif(FileSystemInfo fileInfo)
        {
            var imageFile = ImageFile.FromFile(fileInfo.FullName);
            var inferDateFromExif = imageFile.Properties.Get<ExifDateTime>(ExifTag.DateTime);
            return inferDateFromExif != null ? Prelude.Some(inferDateFromExif.Value) : Prelude.None;
        }

        bool IImageHandler.CanHandle(FileSystemInfo fileInfo) =>
            fileInfo.Extension.ToLower() == ".jpg";

        public void UpdateExif(FileInfo fileInfo, DateTime dateTime)
        {
            var imageFile = ImageFile.FromFile(fileInfo.FullName);
            imageFile.Properties.Set(ExifTag.DateTime, dateTime);
            imageFile.Save(fileInfo.FullName);
        }
    }
}