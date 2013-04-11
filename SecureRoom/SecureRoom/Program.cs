using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Bansky.SPOT.Mail;

namespace SecureRoom
{
    public class Program
    {
        private static OutputPort onBoardLed = new OutputPort(Pins.ONBOARD_LED, false);
        private static PirSensor pir = new PirSensor(Pins.GPIO_PIN_D0);

        public static void Main()
        {
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
        }

        private static void OnSensorTriggered(bool triggered, DateTime time)
        {
            SendEmail(time);
        }

        public static void SendEmail(DateTime time)
        {
            onBoardLed.Write(true);
            MailMessage message = new MailMessage();
            message.From = new MailAddress("yourmail.test@yandex.ru", "Pavel Shchegolevatykh");
            message.To.Add(new MailAddress("yournumber@sms.beemail.ru", "Pavel Shchegolevatykh"));
            message.Subject = "Room Activity Dectected";
            message.Body = "Some movements in your secure room! Time: " + time.ToString() + ".";
            message.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.yourserver.com", 25);
            try
            {
                smtp.Authenticate = true;
                smtp.Username = "yourusername";
                smtp.Password = "yourpassword";
                smtp.Send(message);
            }
            catch (SmtpException e)
            {
                Debug.Print(e.Message);
                Debug.Print("Error Code: " + e.ErrorCode.ToString());
            }
            finally
            {
                smtp.Dispose();
                pir.DiableInterrupt();
            }
            onBoardLed.Write(false);
        }

    }
}
