using System;

namespace Gwent.NET.Exceptions
{
    [Serializable]
    public class GwintException : ApplicationException
    {
        public GwintException()
        {
        }

        public GwintException(string message) : base(message)
        {
        }

        public GwintException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
