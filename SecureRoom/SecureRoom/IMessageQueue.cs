using System;
using Microsoft.SPOT;
using SecureRoom.Domain;
using System.Collections;

namespace SecureRoom
{
    // need this custom interface 
    // instead of using std Queue class
    // because of :
    // * Saving messages to microSD card
    // * Strongly typed methods (do not have generics in MF)
    // * We do not need all the std methods in the API
    public interface IMessageQueue
    {
        void Enqueue(Message message);
        Message Dequeue();
        IEnumerator GetEnumerator();
        Message Peek();
        void Clear();
        int Count { get; }
        void PopulateFromDataStorage();
        void SaveToDataStorage();
    }
}
