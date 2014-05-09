using System;
using Microsoft.SPOT;

namespace SecureRoom.Domain
{
    public class Message
    {
        public string Id { get; set; }
        public string Time { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// Used for existing messages e.g. to reproduce message object after parsing (deserialize).
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="time">Time of detection</param>
        /// <param name="text">Message text</param>
        public Message(string id, string time, string text)
        {
            Id = id;
            Time = time;
            Text = text;
        }


        /// <summary>
        /// Used for newly created messages, new guid will be assigned during creation.
        /// </summary>
        /// <param name="time">Time of detection</param>
        /// <param name="text">Message text</param>
        public Message(string time, string text)
        {
            Id = Guid.NewGuid().ToString();
            Time = time;
            Text = text;
        }

        public override string ToString()
        {
            return Text + " Time: " + Time.ToString() + ".";
        }
    }
}
