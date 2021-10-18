using System;
using System.IO;
using System.Linq;

namespace Lazy.IPhoneBackupsManagement.Old
{
    internal static class FromIPhoneOld
    {
        private const string Whatsapp = "whatsapp";
        private const string Pictures = "jpeg";

        internal static void Run(DirectoryInfo workingDirectory, bool dryRun)
        {
            var workingPath = workingDirectory.FullName;
            
            var files = workingPath
                .AllFiles()
                .Where(f => f.HasExtension("jpg"))
                .Where(f =>
                    !ContainedIn(f, workingPath, Whatsapp) &&
                    !ContainedIn(f, workingPath, Pictures))
                .Select(ExFile.Build)
                .Where(f => f.Valid);


            foreach (var file in files)
            {
                Console.WriteLine($"== Processing: {file.FileInfo.FullName.RelativeTo(workingPath)}");
                try
                {
                    if (file.TakenAt() == null)
                        ElaborateWhatsAppFile(workingPath, file, dryRun);
                    else
                    {
                        ElaboratePictureFile(workingPath, file, dryRun);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static bool ContainedIn(FileInfo f, string workingPath, string dir) =>
            f.Directory.FullName.Contains(
                Path.Join(
                    new DirectoryInfo(workingPath).FullName,
                    dir
                )
            );

        private static void ElaboratePictureFile(string workingPath, ExFile file, bool dryRun)
        {
            var takenAt = file.TakenAt().Value;
            var pictureFilesDir = CreateDirectory(file.FileInfo.DirectoryName, Pictures, dryRun);
            var yearDir = CreateDirectory(pictureFilesDir, takenAt.Year.ToString(), dryRun);
            var monthDir = CreateDirectory(yearDir, $"{takenAt.Month:00}", dryRun);
            var dateDir = CreateDirectory(monthDir, $"{takenAt.Year:0000}-{takenAt.Month:00}-{takenAt.Day:00}", dryRun);

            Console.WriteLine($"{file.FileInfo.FullName.RelativeTo(workingPath)} => {DirectoryExtensions.RelativeTo(dateDir, workingPath)}");
            var destFileName = Path.Join(dateDir, file.FileInfo.Name);
            if(!dryRun)
            {
                File.Move(file.FileInfo.FullName, destFileName);
                File.SetCreationTime(destFileName, takenAt);
                File.SetLastWriteTime(destFileName, takenAt);
                File.SetLastAccessTime(destFileName, takenAt);
            }
        }

        private static void ElaborateWhatsAppFile(string workingPath, ExFile file, bool dryRun)
        {
            Console.WriteLine("WhatsApp file");
            var whatsAppDirectory = CreateWhatsAppDirectoryFor(file, dryRun);

            Move(workingPath, file, whatsAppDirectory, dryRun);
            Console.WriteLine();
        }

        private static void Move(string workingPath, ExFile file, string whatsAppDirectory, bool dryRun)
        {
            var destFileName = Path.Join(whatsAppDirectory, file.FileInfo.Name);
            Console.WriteLine($"{file.FileInfo.FullName.RelativeTo(workingPath)} => {destFileName.RelativeTo(workingPath)}");

            if(!dryRun)
                File.Move(file.FileInfo.FullName, destFileName);
        }

        private static string CreateWhatsAppDirectoryFor(ExFile file, bool dryRun) =>
            CreateDirectory(file.FileInfo.DirectoryName, Whatsapp, dryRun);

        private static string CreateDirectory(string fullPath, string directoryName, bool dryRun)
        {
            var directoryFullName = Path.Join(fullPath, directoryName);
            return directoryFullName.MkDirP(dryRun);
        }

        internal static string MkDirP(this string directory, bool dryRun)
        {
            if (Directory.Exists(directory)) return directory;

            if (!dryRun)
                Directory.CreateDirectory(directory);

            return directory;
        }
    }
}