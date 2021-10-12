using System.Collections.Generic;
using System.Linq;

namespace Lazy.ExifManagement.Commands
{
    internal class CommandBuilder
    {
        private readonly List<ICondition> _conditions;

        internal CommandBuilder(List<ICondition> conditions)
        {
            _conditions = conditions;
        }
        
        internal ICommand CommandFor(MappableImage mappableImage) =>
            _conditions
                .First(c => c.CanHandle(mappableImage))
                .GetCommand(mappableImage);
    }
}