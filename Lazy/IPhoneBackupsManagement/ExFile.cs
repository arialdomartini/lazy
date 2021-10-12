using System;
using System.IO;
using ExifLibrary;

namespace Lazy.IPhoneBackupsManagement
{
    readonly struct ExFile
    {
        private ExFile(FileInfo fileInfo, ImageFile imageFile, bool valid)
        
        {
            FileInfo = fileInfo;
            ImageFile = imageFile;
            Valid = valid;
        }

        internal FileInfo FileInfo { get; }
        internal ImageFile ImageFile { get; }
        internal bool Valid { get; }

        internal static ExFile Build(FileInfo fileInfo)
        {
            Console.WriteLine($"* {fileInfo.FullName}");
            try
            {
                return new ExFile(fileInfo, ImageFile.FromFile(fileInfo.FullName), true);
            }
            catch (Exception)
            {
                return new ExFile(null, null, false);
            }
        }
    }
}