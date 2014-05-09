using System;
using Microsoft.SPOT;

namespace SecureRoom.Logging
{
    /// <summary>
    /// Made this as enum only for code readability.
    /// At run time the values converted to int representation 0, 1, etc.
    /// </summary>
    public enum LogLevel
    {
        INFO,
        ERROR
    };
}
