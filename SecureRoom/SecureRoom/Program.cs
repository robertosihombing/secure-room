using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Bansky.SPOT.Mail;
using PirSensorExample;

namespace SecureRoom
{
    public class Program
    {
        private static OutputPort onBoardLed = new OutputPort(Pins.ONBOARD_LED, false);

        public static void Main()
        {
            PirSensor pir = new PirSensor(Pins.GPIO_PIN_D0);
            pir.SensorTriggered += OnSensorTriggered;

            while (true)
            {
                Thread.Sleep(2000); // just waiting for pir interruptions forever
            }
        }

        static void OnSensorTriggered(bool triggered, DateTime time)
        {
            SendEmail();
        }

        public static void SendEmail()
        {
            onBoardLed.Write(true);
            MailMessage message = new MailMessage();
            message.From = new MailAddress("yourmail.test@yourserver.com", "Pavel Shchegolevatykh");
            message.To.Add(new MailAddress("yournumber@sms.beemail.ru", "Pavel Shchegolevatykh"));
            message.Subject = "Dangerous activity";
            message.Body = "There are some dangerous activity in your secure room!";
            message.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.yoursever.com", 25);
            try
            {
                smtp.Authenticate = true;
                smtp.Username = "yourname";
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
            }
            onBoardLed.Write(false);
        }

    }
}
