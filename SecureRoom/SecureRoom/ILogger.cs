using System;
using Microsoft.SPOT;
using SecureRoom.Logging;

namespace SecureRoom
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
