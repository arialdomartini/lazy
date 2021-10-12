namespace Lazy.ExifManagement
{
    internal interface ICondition
    {
        bool CanHandle(MappableImage mappableImage);
        ICommand GetCommand(MappableImage mappableImage);
    }
}