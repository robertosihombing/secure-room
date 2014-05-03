using System;
using Microsoft.SPOT;
using SecureRoom.Domain;

namespace SecureRoom
{
    public interface ISender
    {
        void Send(string to, Message message);
    }
}
