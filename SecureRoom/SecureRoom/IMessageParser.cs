using System;
using Microsoft.SPOT;
using SecureRoom.Domain;

namespace SecureRoom
{
    public interface IMessageParser
    {
        Message Parse(string textInCertainFormat);
    }
}
