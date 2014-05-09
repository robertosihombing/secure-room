using System;
using Microsoft.SPOT;

namespace SecureRoom.Exceptions
{
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
