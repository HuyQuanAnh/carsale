using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsale.Web.Helper
{
    public static class EmailHelper
    {
        public static void SendEmail(string fromEmail, string toEmail, string subject, string body)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new System.Net.Mail.MailAddress(fromEmail);
            message.To.Add(new System.Net.Mail.MailAddress(toEmail));
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = body;
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Send(message);
        }
    }
}