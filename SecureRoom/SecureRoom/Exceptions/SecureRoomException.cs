using System;
using Microsoft.SPOT;

namespace SecureRoom.Exceptions
{
    /// <summary>
    /// It is more convenient to use custom exceptions for app specific errors
    /// </summary>
    public class SecureRoomException : Exception
    {
        public SecureRoomException()
            : base() { }
        public SecureRoomException(string message)
            : base(message) { }
        public SecureRoomException(string message, Exception inner)
            : base(message, inner) { }
    }
}
