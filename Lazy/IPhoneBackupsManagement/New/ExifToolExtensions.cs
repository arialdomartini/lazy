using System.Collections.Generic;
using System.Linq;
using NExifTool;

namespace Lazy.IPhoneBackupsManagement.New
{
    internal static class ExifToolExtensions{
        internal const string DatetimeOriginal = "DateTimeOriginal";
        private const string CreationDate = "CreateDate";
        private const string GPSDateTime = "GPSDateTime";

        internal static Tag FirstNonNullDate(this IEnumerable<Tag> tags) =>
            FirstNonNullDate(tags.ToList());

        private static Tag FirstNonNullDate(IList<Tag> tags) =>
            tags.FirstOrDefault(l => l.Name == DatetimeOriginal) ?? 
            tags.FirstOrDefault(l => l.Name == CreationDate) ??
            tags.FirstOrDefault(l => l.Name == GPSDateTime);
    }
}