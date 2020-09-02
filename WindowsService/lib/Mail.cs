using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService
{
    public static class Mail
    {
        public static bool Send(string strSubject, string strBody, out string sentMsg)
        {
            try
            {
                var section = (MailPackageValuesSection)ConfigurationManager.GetSection("MailPackages");
                var applications = (from object value in section.Values
                                    select (MailPackageElement)value)
                                    .ToList();

                if (applications != null && applications.Count > 0)
                {
                    var Sender = applications
                        .Where(m => m.Type.ToLower().Trim() == "sender")
                        .FirstOrDefault();

                    var listReceiverEmail = applications.Where(m => m.Type.ToLower().Trim() == "receiver").ToList();

                    foreach (var Receiver in listReceiverEmail)
                    {
                        MailMessage message = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        message.From = new MailAddress(Sender.Email);
                        message.To.Add(new MailAddress(Receiver.Email));
                        message.Subject = strSubject;
                        message.IsBodyHtml = true;
                        message.Body = strBody;
                        smtp.Port = Sender.Port;
                        smtp.Host = Sender.Host;
                        smtp.EnableSsl = Sender.EnableSsl;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(Sender.Email, Sender.Password);
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(message);
                    }
                    sentMsg = "Sent Success";
                    return true;
                }
                else
                {
                    sentMsg = "No mail setting found in app.config";
                    return false;
                }
            }
            catch (Exception ex)
            {
                sentMsg = ex.Message;
                return false;
            }
        }
    }
}
