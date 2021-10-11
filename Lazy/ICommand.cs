namespace Lazy
{
    internal interface ICommand
    {
        void Run(bool dryRun);
    }
}