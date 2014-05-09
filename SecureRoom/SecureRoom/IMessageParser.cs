using System;
using Microsoft.SPOT;
using SecureRoom.Domain;

namespace SecureRoom
{
    /// <summary>
    /// Common interface for message parsers
    /// For now there is only CSV implementation
    /// </summary>
    public interface IMessageParser
    {
        Message Parse(string textInCertainFormat);
    }
}
