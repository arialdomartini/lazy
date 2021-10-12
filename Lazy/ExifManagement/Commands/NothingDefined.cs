using System;

namespace Lazy.ExifManagement.Commands
{
    internal class NothingDefined : ICondition
    {
        bool ICondition.CanHandle(MappableImage mappableImage) =>
            mappableImage.ExifDate.IsNone && 
            mappableImage.FileSystemDate.IsNone;

        ICommand ICondition.GetCommand(MappableImage mappableImage) =>
            new DoNothing(mappableImage);
        
        private class DoNothing : ICommand
        {
            private readonly MappableImage _mappableImage;

            internal DoNothing(MappableImage mappableImage)
            {
                _mappableImage = mappableImage;
            }

            void ICommand.Run(bool dryRun) =>
                Console.WriteLine($"{_mappableImage.FileInfo.FullName}, DoNothing, manual fix");
        }
    }
}