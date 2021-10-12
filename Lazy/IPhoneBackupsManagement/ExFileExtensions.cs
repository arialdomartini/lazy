using ExifLibrary;

namespace Lazy.IPhoneBackupsManagement
{
    internal static class ExFileExtensions
    {
        internal static ExifDateTime TakenAt(this ExFile file) =>
            file.ImageFile.Properties.Get<ExifDateTime>(ExifTag.DateTimeDigitized);
    }
}