using System;

namespace Gwent.NET.Exceptions
{
    [Serializable]
    public class InvalidDeckException : GwintException
    {
        public InvalidDeckException()
        {
        }

        public InvalidDeckException(string message) : base(message)
        {
        }

        public InvalidDeckException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
