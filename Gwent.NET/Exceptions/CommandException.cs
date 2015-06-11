using System;

namespace Gwent.NET.Exceptions
{
    [Serializable]
    public class CommandException : ApplicationException
    {
        public CommandException()
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
