using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Bansky.SPOT.Mail;
using SecureRoom.Ntp;
using SecureRoom.Domain;
using SecureRoom.Logging;
using SecureRoom.Gsm;
using SecureRoom.Config;
using SecureRoom.Smtp;
using System.Collections;
using SecureRoom.Storage;
using SecureRoom.Csv;

namespace SecureRoom
{
    public class Program
    {
        private static readonly OutputPort onBoardLed = new OutputPort(Pins.ONBOARD_LED, false);
        private static readonly PirSensor pir = new PirSensor(Pins.GPIO_PIN_D8);
        private static readonly ILogger logger = new CombinedLogger();
        private static readonly ISender smsSender = new SmsSender();
        private static readonly ISender emailSender = new EmailSender();
        private static readonly IMessageQueue emailMessageQueue = 
            new StorableMessageQueue(new MicroSdMessageQueueRepository(Settings.EmailMessageQueueFilePath,
                    new CsvMessageParser()));
        private static readonly IMessageQueue smsMessageQueue = 
            new StorableMessageQueue(new MicroSdMessageQueueRepository(Settings.SmsMessageQueueFilePath,
                new CsvMessageParser()));

        public static void Main()
        {
            bool timeUpdated = NtpHelper.UpdateTimeFromNtpServer(Settings.NtpServerAddress, Settings.UtcTimeShift); 
            if (timeUpdated)
            {
                logger.Log(LogLevel.INFO, "Time was successfully updated.");
            }
            else
            {
                logger.Log(LogLevel.ERROR, "Time was not updated.");
            }
            // timer starts immidiately after creation
            Timer interruptTimer = new Timer(OnInterruptTimer, null, 0, Settings.InterruptionsTimerPeriod);
            pir.SensorTriggered += OnSensorTriggered;

            Timer messageQueueTimer = new Timer(OnMessageQueueTimer, null, 0, Settings.MessageQueueTimerPeriod);
    
            while (true)
            {
                Thread.Sleep(2000); // just waiting for pir interruptions forever
            }
        }

        private static void OnMessageQueueTimer(Object state)
        {
            ProcessSmsMQ();
            ProcessEmailMQ();
        }

        private static void ProcessEmailMQ()
        {
            try
            {
                emailMessageQueue.PopulateFromDataStorage();
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.ERROR, e.ToString());
                logger.Log(LogLevel.ERROR, "Unable to get Email messages from microSD card.");
            }

            while (emailMessageQueue.Count != 0)
            {
                Message emailMessage = null;
                try
                {
                    emailMessage = emailMessageQueue.Dequeue();
                }
                catch (Exception e)
                {
                    logger.Log(LogLevel.ERROR, e.ToString());
                    logger.Log(LogLevel.ERROR, "Can not get Email message from the queue.");
                }
                if (emailMessage != null)
                {
                    SendEmail(emailMessage);
                }
            }
        }

        private static void ProcessSmsMQ()
        {
            try
            {
                smsMessageQueue.PopulateFromDataStorage();
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.ERROR, e.ToString());
                logger.Log(LogLevel.ERROR, "Unable to get SMS messages from microSD card.");
            }

            while (smsMessageQueue.Count != 0)
            {
                Message smsMessage = null;
                try
                {
                    smsMessage = smsMessageQueue.Dequeue();
                }
                catch (Exception e)
                {
                    logger.Log(LogLevel.ERROR, e.ToString());
                    logger.Log(LogLevel.ERROR, "Can not get SMS message from the queue.");
                }
                if (smsMessage != null)
                {
                    SendSms(smsMessage);
                }
            }
        }

        private static void OnInterruptTimer(Object state)
        {
            pir.EnableInterrupt();
#if DEBUG
            Debug.Print("PIR sensor enabled.");
#endif
        }

        private static void OnSensorTriggered(bool triggered, DateTime time)
        {
            logger.Log(LogLevel.INFO, "Movement detected!");
            Message messageToSend = new Message(time.ToString(), Settings.MessageText);
            SendSms(messageToSend);
            SendEmail(messageToSend);
        }

        private static void SendEmail(Message messageToSend)
        {
            onBoardLed.Write(true);
            try
            {
                logger.Log(LogLevel.INFO, "Sending Email message.");
                emailSender.Send(Settings.ReceiverEmailAddress, messageToSend);
                logger.Log(LogLevel.INFO, "Email message was sent.");
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.ERROR, e.Message);
                logger.Log(LogLevel.ERROR, "Email message was not sent.");
                emailMessageQueue.Enqueue(messageToSend);
                try
                {
                    emailMessageQueue.SaveToDataStorage();
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.ERROR, ex.Message);
                    logger.Log(LogLevel.ERROR, "Email message queue was not saved.");
                }
            }
            finally
            {
#if DEBUG
                pir.DisableInterrupt();
#endif
            }
            onBoardLed.Write(false);
        }

        private static void SendSms(Message messageToSend)
        {
            onBoardLed.Write(true);
            try
            {
                logger.Log(LogLevel.INFO, "Sending SMS message.");
                smsSender.Send(Settings.PhoneNumber, messageToSend);
                logger.Log(LogLevel.INFO, "SMS message was sent.");
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.ERROR, e.Message);
                logger.Log(LogLevel.ERROR, "SMS message was not sent.");
                smsMessageQueue.Enqueue(messageToSend);
                try
                {
                    smsMessageQueue.SaveToDataStorage();
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.ERROR, ex.Message);
                    logger.Log(LogLevel.ERROR, "SMS message queue was not saved.");
                }
            }
            finally
            {
                pir.DisableInterrupt();
#if DEBUG
                Debug.Print("PIR sensor disabled.");
#endif
            }
            onBoardLed.Write(false);
        }
    }
}
