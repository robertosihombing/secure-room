using System;
using Microsoft.SPOT;
using SecureRoom.Domain;
using Bansky.SPOT.Mail;
using SecureRoom.Config;

namespace SecureRoom.Smtp
{
    public class EmailSender : ISender
    {
        public void Send(string to, Message message)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Settings.SenderEmailAddress, "Security System");
            mailMessage.To.Add(new MailAddress(to, "Receiver Person"));
            mailMessage.Subject = "Room Activity Dectected";
            mailMessage.Body = message.ToString();
            mailMessage.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient(Settings.SmtpServer, Settings.SmtpPort);
            try
            {
                smtp.Authenticate = true;
                smtp.Username = Settings.SmtpUsername;
                smtp.Password = Settings.SmtpPassword;
                smtp.Send(mailMessage);
            }
            catch (SmtpException e)
            {
                Debug.Print(e.Message);
                Debug.Print("Error Code: " + e.ErrorCode.ToString());
                throw;
            }
            finally
            {
                smtp.Dispose();
            }
            
        }

        
    }
}
