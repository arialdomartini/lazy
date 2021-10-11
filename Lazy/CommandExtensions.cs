using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Lazy
{
    internal static class CommandExtensions{
        internal static Command Create(string name, string description, Action<bool> handler)
        {
            Command command = new(
                name,
                description)
            {
                new Option("--dry-run", "Do not perform any change")
            };

            command.Handler = CommandHandler.Create(handler);
            
            return command;
        }
    }
}