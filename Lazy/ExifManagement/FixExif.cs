using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazy.ExifManagement.Commands;
using Lazy.ExifManagement.ImageHandlers;

namespace Lazy.ExifManagement
{
    internal class FixExif
    {
        private readonly RootImageHandler _rootImageHandler;
        private readonly CommandBuilder _commandBuilder;

        internal FixExif(RootImageHandler rootImageHandler)
        {
            _rootImageHandler = rootImageHandler;
            _commandBuilder = new CommandBuilder(new List<ICondition>
            {
                new BothDefined(_rootImageHandler),
                new NothingDefined(),
                new OnlyExifDefined(),
                new OnlyFileSystemDefined(_rootImageHandler)
            });
        }

        internal void Run(DirectoryInfo workingDirectory, bool dryRun)
        {
            workingDirectory
                .GetImages(_rootImageHandler)
                .Select(_commandBuilder.ToCommand)
                .ToList()
                .ForEach(c =>
                {
                    c.Run(dryRun);
                });
        }
    }
}