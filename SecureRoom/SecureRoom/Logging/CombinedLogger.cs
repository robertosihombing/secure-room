using System;
using Microsoft.SPOT;

namespace SecureRoom.Logging
{
    public class CombinedLogger : ILogger
    {

        public void Log(LogLevel level, string message)
        {
            Debug.Print(level.ToString() + ": " + message);
            // TODO: add logging on a flash drive as well
        }
    }
}
