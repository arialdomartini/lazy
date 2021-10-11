using ExifLibrary;

namespace Lazy
{
    internal static class ExFileExtensions
    {
        internal static ExifDateTime TakenAt(this ExFile file) =>
            file.ImageFile.Properties.Get<ExifDateTime>(ExifTag.DateTimeDigitized);
    }
}