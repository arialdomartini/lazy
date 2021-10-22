using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazy.ExifManagement.Commands;
using Lazy.ExifManagement.ImageHandlers;

namespace Lazy.ExifManagement
{
    internal class FixExif
    {
        private readonly RootImageHandler RootImageHandler;
        private readonly CommandBuilder CommandBuilder;

        internal FixExif()
        {
            RootImageHandler = new RootImageHandler();
            CommandBuilder = new CommandBuilder(new List<ICondition>
            {
                new BothDefined(RootImageHandler),
                new NothingDefined(),
                new OnlyExifDefined(),
                new OnlyFileSystemDefined(RootImageHandler)
            });
        }

        internal void Run(DirectoryInfo workingDirectory, bool dryRun)
        {
            workingDirectory
                .GetImages(RootImageHandler)
                .Select(CommandBuilder.ToCommand)
                .ToList()
                .ForEach(c =>
                {
                    c.Run(dryRun);
                });
        }
    }
}