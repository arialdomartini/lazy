using System.Collections.Generic;
using System.IO;

namespace Lazy
{
    internal static class DirectoryInfoExtensions
    {
        internal static IEnumerable<FileInfo> AllFiles(this DirectoryInfo directory) =>
            directory
                .EnumerateFiles("*.*", SearchOption.AllDirectories);
    }
}