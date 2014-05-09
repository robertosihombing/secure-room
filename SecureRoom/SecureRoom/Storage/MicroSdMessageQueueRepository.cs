using System;
using Microsoft.SPOT;
using SecureRoom.Domain;
using System.Collections;
using System.IO;
using SecureRoom.Config;
using SecureRoom.Exceptions;

namespace SecureRoom.Storage
{
    public class MicroSdMessageQueueRepository : IMessageQueueRepository
    {
        private readonly string queueFilePath;
        private readonly IMessageParser messageParser;

        public MicroSdMessageQueueRepository(string queueFilePath, IMessageParser messageParser)
        {
            this.queueFilePath = queueFilePath;
            this.messageParser = messageParser;
        }

        public void Save(Queue messages)
        {
            using (var fileStream = new FileStream(queueFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    foreach (object item in messages)
                    {
                        if (item is Message)
                        {
                            Message message = item as Message;
                            // order is important
                            writer.WriteLine(message.Id + "," + message.Time + "," + message.Text);
                        }
                        else
                        {
                            throw new SecureRoomException("Object in a queue is not a message.");
                        }
                    }
                    
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        public Queue Get()
        {
            Queue messageQueue = new Queue();

            if (!File.Exists(queueFilePath))
            {
                throw new SecureRoomException("Message queue file does not exist.");
            }
            using (var fileStream = new FileStream(queueFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    string content = reader.ReadToEnd();
                    string[] lines = content.Split('\n');
                    foreach (string line in lines)
                    {
                        Message parsedMessage = messageParser.Parse(line);
                        messageQueue.Enqueue(parsedMessage);
                        reader.Close();
                    }
                }

            }

            return messageQueue;
        }
    }
}
