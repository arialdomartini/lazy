namespace Lazy.IPhoneBackupsManagement.New
{
    internal interface IOperation
    {
        string Run(bool dryRun);
    }
}