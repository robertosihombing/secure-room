using System;
using Microsoft.SPOT;

namespace SecureRoom.Config
{
    public static class Settings
    {
        private static readonly string phoneNumber = "+79056511516";
        private static readonly string messageText = "Detected some movements in the room!";
        private static readonly string receiverEmailAddress = "79056511516@sms.beemail.ru";
        private static readonly string senderEmailAddress = "secure.room@yandex.ru";
        private static readonly string smtpServer = "smtp.yandex.ru";
        private static readonly int smtpPort = 587;
        private static readonly string smtpUsername = "secure.room";
        private static readonly string smtpPassword = "bluesky";

        public static string SmtpUsername
        {
            get { return smtpUsername; }
        }
        public static string SmtpPassword
        {
            get { return Settings.smtpPassword; }
        } 
        public static string SmtpServer
        {
            get { return smtpServer; }
        } 
        public static int SmtpPort
        {
            get { return smtpPort; }
        } 
        public static string SenderEmailAddress
        {
            get { return senderEmailAddress; }
        }
        public static string ReceiverEmailAddress
        {
            get { return receiverEmailAddress; }
        }
        public static string MessageText
        {
            get { return messageText; }
        }
        public static string PhoneNumber
        {
            get { return phoneNumber; }
        } 
    }
}