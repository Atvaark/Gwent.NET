using System;

namespace Gwent.NET.Exceptions
{
    [Serializable]
    public class CommandException : GwintException
    {
        private const string DefaultMessage = "Command was rejected by the server.";

        public CommandException() : base(DefaultMessage)
        {
        }

        public CommandException(string message) : base(message)
        {
        }

        public CommandException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
