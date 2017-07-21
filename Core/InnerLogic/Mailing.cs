using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using ViewModels.General;

namespace Core.InnerLogic
{
    public class Mailing
    {
        public void SendForgotPasswordmail(string password, GeneralUser user)
        {

        }
        public void Sendmail(int emailType = 1)
        {

        }

        public void Sendmail(Exception ex)
        {
            string body = "Error guardando archivo \r \n ";
            body += ex.HResult.ToString() + " \r \n ";
            body += ex.InnerException + " \r \n ";
            body += ex.Message + " \r \n ";
            body += ex.Source + " \r \n ";
            body += ex.TargetSite + " \r \n ";
            body += ex.HelpLink + " \r \n ";
            body += DateTime.Now + " \r \n ";
            var subject = string.Concat("Error guardando archivo");
            SendEmail(new MailAddress(WebConfigurationManager.AppSettings["from"],
                WebConfigurationManager.AppSettings["fromName"]), new MailAddress("omar.moreno@lotusodonto.com",
                "Omar Moreno"), body, subject);
        }

        private void SendEmail(MailAddress from, MailAddress to, string body, string subject)
        {
            try
            {
                var smtp = new SmtpClient
                {
                    Host = WebConfigurationManager.AppSettings["SmtpHost"],
                    Port = Convert.ToInt32(WebConfigurationManager.AppSettings["SmtpPort"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["from"],
                        WebConfigurationManager.AppSettings["Password"]),
                    Timeout = 10000
                };
                using (var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8,
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}