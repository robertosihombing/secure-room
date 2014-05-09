using System;
using Microsoft.SPOT;
using SecureRoom.Logging;

namespace SecureRoom
{
    /// <summary>
    /// Common interface allows us to change actual logger implementation
    /// without affecting that much code
    /// </summary>
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
