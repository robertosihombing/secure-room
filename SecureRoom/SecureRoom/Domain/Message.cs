using System;
using Microsoft.SPOT;

namespace SecureRoom.Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }

        public Message(DateTime time, string text)
        {
            Id = Guid.NewGuid();
            Time = time;
            Text = text;
        }

        public override string ToString()
        {
            return Text + " Time: " + Time.ToString() + ".";
        }
    }
}
