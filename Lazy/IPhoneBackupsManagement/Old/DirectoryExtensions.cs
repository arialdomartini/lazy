using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lazy.IPhoneBackupsManagement.Old
{
    internal static class DirectoryExtensions
    {
        internal static IEnumerable<FileInfo> AllFiles(this string path) =>
            Directory
                .EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f));

        internal static bool HasExtension(this FileInfo fileInfo, string extension) =>
            fileInfo.Extension.ToLower() == $".{extension}";

        internal static string RelativeTo(this string fullPath, string path) =>
            Path.GetRelativePath(path, fullPath);
    }
}