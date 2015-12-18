using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;

namespace HealthCheck
{
    public class EmailClient
    {
        public void Send(string statusMessage)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(ConfigurationManager.AppSettings["emailFrom"]);

            var emailRecipients = ConfigurationManager.AppSettings["emailRecipients"]
                .Split(';')
                .Select(url => url.Replace(Environment.NewLine, "").Trim())
                .ToList();

            foreach (var address in emailRecipients)
            {
                mail.To.Add(address);
            }

            var client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = ConfigurationManager.AppSettings["emailHost"];
            mail.Subject = ConfigurationManager.AppSettings["emailSubject"];
            mail.Body = statusMessage;
            mail.IsBodyHtml = true;
            client.Send(mail);
        }
    }
}