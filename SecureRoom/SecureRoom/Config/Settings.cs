using System;
using Microsoft.SPOT;

namespace SecureRoom.Config
{
    /// <summary>
    /// All the options in one place
    /// </summary>
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
        // most reliable server for now, could be changed
        private static readonly string ntpServerAddress = "time.nist.gov";
        // UTC + 4 = Moscow time
        private static readonly int utcTimeShift = 4;
        // 2 mins for interruptions and 4 mins for message queue
        private static readonly int interruptionsTimerPeriod = 120000;
        private static readonly int messageQueueTimerPeriod = 240000;
        // \SD\ is a root folder of microSD card
        private static readonly string emailMessageQueueFilePath = @"\SD\EmailMessages.csv";
        private static readonly string smsMessageQueueFilePath = @"\SD\SmsMessages.csv";
        private static readonly string loggerFilePath = @"\SD\SecureRoom.log";

        public static string EmailMessageQueueFilePath
        {
            get { return emailMessageQueueFilePath; }
        }

        public static string SmsMessageQueueFilePath
        {
            get { return smsMessageQueueFilePath; }
        }

        public static string LoggerFilePath
        {
            get { return loggerFilePath; }
        }
 
        public static int InterruptionsTimerPeriod
        {
            get { return interruptionsTimerPeriod; }
        }

        public static int MessageQueueTimerPeriod
        {
            get { return messageQueueTimerPeriod; }
        }

        public static int UtcTimeShift
        {
            get { return utcTimeShift; }
        }
 
        public static string NtpServerAddress
        {
            get { return ntpServerAddress; }
        }
 
        public static string SmtpUsername
        {
            get { return smtpUsername; }
        }

        public static string SmtpPassword
        {
            get { return smtpPassword; }
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
