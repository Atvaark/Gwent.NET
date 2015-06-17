using System;

namespace Gwent.NET.Exceptions
{
    [Serializable]
    public class CommandParseException : GwintException
    {
        private const string DefaultMessage = "Unable to process the command.";

        public CommandParseException() : this(DefaultMessage)
        {
        }

        public CommandParseException(string message) : base(message)
        {
        }

        public CommandParseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
