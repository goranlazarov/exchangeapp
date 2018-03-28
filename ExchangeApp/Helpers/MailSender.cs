using DnsClient;
using ExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExchangeApp.Helpers
{
    public static class MailSender
    {
        private static string From = ConfigurationManager.AppSettings["EmailAddress"];
        private static string Password = ConfigurationManager.AppSettings["Password"];
        private static string CCMail = ConfigurationManager.AppSettings["CCEmailAddress"];
        private static string Host = ConfigurationManager.AppSettings["Host"];
        private static bool EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
        private static int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        public static StudentViewModel Model { get; set; }



        public static void SendMails()
        {
            string from = ConfigurationManager.AppSettings["EmailAddress"]; //"mobilityacbsp@gmail.com"; //model.Email;
            string password = ConfigurationManager.AppSettings["Password"]; //"brooklyn78";//"1234qawsed";
            string ccMail = ConfigurationManager.AppSettings["CCEmailAddress"];

            MailMessage confirmationMail = PrepareMail(true);
            MailMessage applicationMail = PrepareMail(false);

            NetworkCredential networkCredential = new NetworkCredential(from, password);

            GetSmtp(networkCredential).Send(confirmationMail);
            GetSmtp(networkCredential).Send(applicationMail);
        }

        private static MailMessage PrepareMail(bool isConfirmation)
        {

            MailMessage mail = new MailMessage(From, (!isConfirmation ? Model.Faculty.Email : Model.Email));
            if (!isConfirmation)
            {
                MailAddress ccMailAddress = new MailAddress(CCMail);
                mail.CC.Add(ccMailAddress);

                mail.Subject = ConfigurationManager.AppSettings["ApplicationMailSubject"];// "ACBSP mobility application from";
                string pathFile = Model.FacultySelected ? @"~/App_Data/MailBodyApplicationFaculty.txt" : @"~/App_Data/MailBodyApplicationStudent.txt";
                var fileContents = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(pathFile));
                mail.IsBodyHtml = false;
                mail.Body = SetUpBody(fileContents);
                Attachment messageAttachment = new Attachment(Model.CV.InputStream, Model.CV.FileName);
                mail.Attachments.Add(messageAttachment);
            }
            else
            {
                mail.Subject = ConfigurationManager.AppSettings["ConfirmationMailSubject"];// "ACBSP mobility confirmation for successfull application";
                string pathFile = Model.FacultySelected ? @"~/App_Data/MailBodyConfirmationFaculty.txt" : @"~/App_Data/MailBodyConfirmationStudent.txt";
                var fileContents = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(pathFile));
                mail.IsBodyHtml = false;
            }

            return mail;
        }

        private static string SetUpBody(string content)
        {
            content = content.Replace("(Gender)", Model.Gender).Replace("(FirstName)", Model.FirstName)
                             .Replace("FirstName", Model.FirstName).Replace("LastName", Model.LastName)
                             .Replace("FirstName", Model.FirstName).Replace("LastName", Model.LastName)
                             .Replace("FirstName", Model.FirstName).Replace("LastName", Model.LastName);
            return content;

        }

        private static SmtpClient GetSmtp(NetworkCredential networkCredential)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["Host"];
            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            return smtp;
        }

        public static Task<bool> IsValidAsync()
        {
            try
            {
                var mailAddress = new MailAddress(Model.Email);
                var host = mailAddress.Host;
                return CheckDnsEntriesAsync(host);
            }
            catch (FormatException)
            {
                return Task.FromResult(false);
            }
        }

        public static async Task<bool> CheckDnsEntriesAsync(string domain)
        {
            try
            {
                var lookup = new LookupClient();
                lookup.Timeout = TimeSpan.FromSeconds(5);
                var result = await lookup.QueryAsync(domain, QueryType.ANY).ConfigureAwait(false);

                var records = result.Answers.Where(record => record.RecordType == DnsClient.Protocol.ResourceRecordType.A ||
                                                             record.RecordType == DnsClient.Protocol.ResourceRecordType.AAAA ||
                                                             record.RecordType == DnsClient.Protocol.ResourceRecordType.MX);
                return records.Any();
            }
            catch (DnsResponseException)
            {
                return false;
            }
        }

        public static bool IsValidEmail()
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Model.Email);
                return addr.Address == Model.Email;
            }
            catch
            {
                return false;
            }
        }
    }
}