using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace Lazy.CommandLine
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
        
        internal static Command CreateWithOutput(string name, string description, Action<bool, DirectoryInfo> handler)
        {
            Command command = new(
                name,
                description)
            {
                new Option("--dry-run", "Do not perform any change"),
                new Option<DirectoryInfo>("--output", "The directory the files will be moved to")
            };

            command.Handler = CommandHandler.Create(handler);
            
            return command;
        }
    }
}