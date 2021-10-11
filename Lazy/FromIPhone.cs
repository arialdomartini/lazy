using System;
using System.IO;
using System.Linq;

namespace Lazy
{
    internal static class FromIPhone
    {
        private const string Whatsapp = "whatsapp";
        private const string Pictures = "jpeg";
        private const bool DryRun = true;

        internal static void Run(DirectoryInfo workingDirectory)
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
                        ElaborateWhatsAppFile(workingPath, file);
                    else
                    {
                        ElaboratePictureFile(workingPath, file);
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

        private static void ElaboratePictureFile(string workingPath, ExFile file)
        {
            var takenAt = file.TakenAt().Value;
            var pictureFilesDir = CreateDirectory(workingPath, file.FileInfo.DirectoryName, Pictures);
            var yearDir = CreateDirectory(workingPath, pictureFilesDir, takenAt.Year.ToString());
            var monthDir = CreateDirectory(workingPath, yearDir, $"{takenAt.Month:00}");
            var dateDir = CreateDirectory(workingPath, monthDir, $"{takenAt.Year:0000}-{takenAt.Month:00}-{takenAt.Day:00}");

            Console.WriteLine($"{file.FileInfo.FullName.RelativeTo(workingPath)} => {DirectoryExtensions.RelativeTo(dateDir, workingPath)}");
            var destFileName = Path.Join((string?) dateDir, file.FileInfo.Name);
            if(!DryRun)
            {
                File.Move(file.FileInfo.FullName, destFileName);
                File.SetCreationTime(destFileName, takenAt);
                File.SetLastWriteTime(destFileName, takenAt);
                File.SetLastAccessTime(destFileName, takenAt);
            }
        }

        private static void ElaborateWhatsAppFile(string workingPath, ExFile file)
        {
            Console.WriteLine("WhatsApp file");
            var whatsAppDirectory = CreateWhatsAppDirectoryFor(workingPath, file);

            Move(workingPath, file, whatsAppDirectory);
            Console.WriteLine();
        }

        private static void Move(string workingPath, ExFile file, string whatsAppDirectory)
        {
            var destFileName = Path.Join(whatsAppDirectory, file.FileInfo.Name);
            Console.WriteLine($"{file.FileInfo.FullName.RelativeTo(workingPath)} => {destFileName.RelativeTo(workingPath)}");

            if(!DryRun)
                File.Move(file.FileInfo.FullName, destFileName);
        }

        private static string CreateWhatsAppDirectoryFor(string workingPath, ExFile file) =>
            CreateDirectory(workingPath, file.FileInfo.DirectoryName, Whatsapp);

        private static string CreateDirectory(string workingPath, string fullPath, string directoryName)
        {
            var directoryFullName = Path.Join(fullPath, directoryName);
            if (Directory.Exists(directoryFullName)) return directoryFullName;

            Console.WriteLine($"mkdir {directoryFullName.RelativeTo(workingPath)}");

            if (!DryRun)
                Directory.CreateDirectory(directoryFullName);

            return directoryFullName;
        }
    }
}