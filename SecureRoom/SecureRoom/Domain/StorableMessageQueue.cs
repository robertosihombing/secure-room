using System;
using Microsoft.SPOT;
using System.Collections;
using SecureRoom.Exceptions;

namespace SecureRoom.Domain
{
    /// <summary>
    /// Wrapper for the .NET queue class with our necessary functions
    /// </summary>
    public class StorableMessageQueue : IMessageQueue
    {
        private readonly IMessageQueueRepository messageRepository;
        private Queue internalMessageQueue = new Queue();
        
        /// <summary>
        /// Takes repository into constructor, no hard dependencies, so the storage could be replaced
        /// </summary>
        /// <param name="messageRepository">Storage implementation for messages</param>
        public StorableMessageQueue(IMessageQueueRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        public void Enqueue(Message message)
        {
            this.internalMessageQueue.Enqueue(message);
        }

        public Message Dequeue()
        {
            if (internalMessageQueue.Count != 0)
            {
                object objectInAQueue = internalMessageQueue.Dequeue();
                
                if (objectInAQueue is Message)
                {
                    return objectInAQueue as Message;
                }
                throw new SecureRoomException("Object in a queue is not a message.");
            }
            throw new SecureRoomException("Message queue is empty.");
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return internalMessageQueue.GetEnumerator();
        }

        public Message Peek()
        {
            if (internalMessageQueue.Count != 0)
            {
                object objectInAQueue = internalMessageQueue.Peek();

                if (objectInAQueue is Message)
                {
                    return objectInAQueue as Message;
                }
                throw new SecureRoomException("Object in a queue is not a message.");
            }
            throw new SecureRoomException("Message queue is empty.");
        }

        public void Clear()
        {
            internalMessageQueue.Clear();
        }

        public int Count
        {
            get { return internalMessageQueue.Count; }
        }

        public void PopulateFromDataStorage()
        {
            internalMessageQueue = messageRepository.Get();
        }

        public void SaveToDataStorage()
        {
            messageRepository.Save(internalMessageQueue);
        }
    }
}
