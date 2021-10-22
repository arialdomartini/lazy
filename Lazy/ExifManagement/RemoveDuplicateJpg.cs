using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lazy.ExifManagement
{
    internal static class RemoveDuplicateJpg
    {
        public static void Run(DirectoryInfo workingDirectory, bool dryRun)
        {
            var groupBy = workingDirectory
                .AllFiles()
                .Where(f => f.Extension.ToLower() is ".jpg" or ".heic")
                .ToLookup(f => f.Extension.ToLower());

            var jpgs = groupBy[".jpg"];
            var heics = groupBy[".heic"];
            
            var toBeDeleted = jpgs.Select(j=>(Jpg:j, Heic:EquivalentHeic(j, heics))).Where(t => t.Heic!=null);
            
            Console.WriteLine($"Found {toBeDeleted.Count()} to be deleted");
            foreach (var tuple in toBeDeleted)
            {
                Console.WriteLine($"{tuple.Jpg.Name} / {tuple.Heic.Name}");
            }
        }

        private static FileInfo EquivalentHeic(FileSystemInfo jpg, IEnumerable<FileInfo> heics) =>
            heics.SingleOrDefault(h => h.HasSameNameOf(jpg));

        private static bool HasSameNameOf(this FileSystemInfo h, FileSystemInfo jpg) =>
            h.NameWithoutExtension() == jpg.NameWithoutExtension();

        private static string NameWithoutExtension(this FileSystemInfo h) =>
            Path.GetFileNameWithoutExtension(h.Name);
    }
}