namespace Lazy.ExifManagement
{
    internal interface ICommand
    {
        void Run(bool dryRun);
    }
}