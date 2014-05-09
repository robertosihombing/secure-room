using System;
using Microsoft.SPOT;
using SecureRoom.Domain;
using SecureRoom.Exceptions;

namespace SecureRoom.Csv
{
    public class CsvMessageParser : IMessageParser
    {
        public Message Parse(string textInCsvFormat)
        {
            string[] messageDetails = textInCsvFormat.Split(',');
            try
            {
                string id = messageDetails[0];
                string time = messageDetails[1];
                string text = messageDetails[2];
                return new Message(id, time, text);
            }
            catch (Exception e)
            {
                throw new SecureRoomException("Message format was incorrect.", e);
            }
        }
    }
}
