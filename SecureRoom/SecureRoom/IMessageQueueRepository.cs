using System;
using Microsoft.SPOT;
using SecureRoom.Domain;
using System.Collections;

namespace SecureRoom
{
    /// <summary>
    /// Common iterface for MessageQueueRepository
    /// It allows to mock the actual implementation for testing purposes
    /// </summary>
    public interface IMessageQueueRepository
    {
        void Save(Queue messages);
        Queue Get();
    }
}
