using System;
using System.IO;
using LanguageExt;
using LibHeifSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace Lazy.ImageHandlers
{
    internal class HeifImageHandler : IImageHandler
    {
        Option<DateTime> IImageHandler.ReadDateFromExif(FileSystemInfo imageFile)
        {
            var heifContext = new HeifContext(imageFile.FullName);
            var topLevelImageIds = heifContext.GetTopLevelImageIds();
            var heifImageHandle = heifContext.GetImageHandle(topLevelImageIds[0]);
            var exifMetadata = heifImageHandle.GetExifMetadata();
            var exifProfile = new ExifProfile(exifMetadata);
            var exifValue = exifProfile.GetValue(ExifTag.DateTime);
            var exifValueValue = exifValue.Value;

            return DateTime.TryParse(exifValueValue, out var date)? 
                Prelude.Some(date) : 
                Prelude.None;
        }
        
        bool IImageHandler.CanHandle(FileSystemInfo fileInfo) =>
            fileInfo.Extension.ToLower() == ".heic";

        public void UpdateExif(FileInfo fileInfo, DateTime dateTime) =>
            Console.WriteLine("Will update Exif date of HEIF file");
    }
}