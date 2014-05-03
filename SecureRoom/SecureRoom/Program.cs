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

namespace SecureRoom
{
    public class Program
    {
        private static OutputPort onBoardLed = new OutputPort(Pins.ONBOARD_LED, false);
        private static PirSensor pir = new PirSensor(Pins.GPIO_PIN_D8);
        private static readonly ILogger logger = new CombinedLogger();
        private static readonly ISender smsSender = new SmsSender();
        private static readonly ISender emailSender = new EmailSender();

        public static void Main()
        {
            bool timeUpdated = NtpHelper.UpdateTimeFromNtpServer("time.nist.gov", 4); // UTC + 4 = Moscow time
            if (timeUpdated)
            {
                logger.Log(LogLevel.INFO, "Time was successfully updated.");
            }
            else
            {
                logger.Log(LogLevel.ERROR, "Time was not updated.");
            }
 
            Timer interruptTimer = new Timer(OnInterruptTimer, null, 0, 60000);
            pir.SensorTriggered += OnSensorTriggered;

            while (true)
            {
                Thread.Sleep(2000); // just waiting for pir interruptions forever
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
            Message messageToSend = new Message(time, Settings.MessageText);
            //SendSms(messageToSend);
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
