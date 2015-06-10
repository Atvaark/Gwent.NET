using System;

namespace Gwent.NET.Exceptions
{
    [Serializable]
    public class InvalidCommandException : ApplicationException
    {
        public InvalidCommandException()
        {
        }

        public InvalidCommandException(string message) : base(message)
        {
        }

        public InvalidCommandException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
