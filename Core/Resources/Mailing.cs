using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Core.DataBase;
using LotusViewModels.General;
using ViewModels.ILotusAssistance;

namespace Core.Resources
{
    public class Mailing
    {
        private readonly GeneralDb _db = new GeneralDb();

        public void SendCreationEmail(User user)
        {
            try
            {
                string body;
                using (var client = new WebClient())
                {
                    body = client.DownloadString("http://logos.lotusodonto.com/WelcomeLotusApps.html");
                }
                var query = "exec sp_GetCompany '" + user.UserId + "', '1'";
                var company = _db.Database.SqlQuery<Empresas>(query).FirstOrDefault();
                body = body.Replace("@CompanyUrl", company.url);
                body = body.Replace("@Company", company.name);
                body = body.Replace("@password", user.Password);
                body = body.Replace("@email", user.UserEmail);
                body = body.Replace("@AppUrl", WebConfigurationManager.AppSettings["SiteUrl"]);
                var subject = string.Concat("Welcome ", user.Name, " ", user.UserLastname);
                SendEmail(new MailAddress(WebConfigurationManager.AppSettings["from"],
                    WebConfigurationManager.AppSettings["fromName"]), new MailAddress(user.UserEmail,
                    string.Concat(user.Name, " ", user.UserLastname)), body, subject);
            }
            catch (Exception e)
            {
                //
            }
        }

        public void SendUpdateEmail(User user)
        {
            try
            {
                string body;
                using (var client = new WebClient())
                {
                    body = client.DownloadString("http://logos.lotusodonto.com/PasswordRecoveryLotusApps.html");
                }
                var query = "exec sp_GetCompany '" + user.UserId + "', '1'";
                var company = _db.Database.SqlQuery<Empresas>(query).FirstOrDefault();
                body = body.Replace("@CompanyUrl", company.url);
                body = body.Replace("@Company", company.name);
                body = body.Replace("@password", user.Password);
                body = body.Replace("@email", user.UserEmail);
                body = body.Replace("@AppUrl", WebConfigurationManager.AppSettings["SiteUrl"]);
                var subject = string.Concat("Password Recovery ", user.Name, " ", user.UserLastname);
                SendEmail(new MailAddress(WebConfigurationManager.AppSettings["from"],
                    WebConfigurationManager.AppSettings["fromName"]), new MailAddress(user.UserEmail,
                    string.Concat(user.Name, " ", user.UserLastname)), body, subject);
            }
            catch (Exception)
            {
                //
            }
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