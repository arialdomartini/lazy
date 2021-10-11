namespace Lazy
{
    internal interface ICondition
    {
        bool CanHandle(MappableImage mappableImage);
        ICommand GetCommand(MappableImage mappableImage);
    }
}