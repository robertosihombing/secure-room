using System;
using Microsoft.SPOT;
using SecureRoom.Domain;
using System.Collections;

namespace SecureRoom
{
    public interface IMessageQueueRepository
    {
        void Save(Queue messages);
        Queue Get();
    }
}
