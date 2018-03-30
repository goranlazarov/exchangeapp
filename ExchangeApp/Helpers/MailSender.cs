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
        private static string ApplicationMailSubject = ConfigurationManager.AppSettings["ApplicationMailSubject"];
        private static string ConfirmationMailSubject = ConfigurationManager.AppSettings["ConfirmationMailSubject"];
        public static StudentViewModel Model { get; set; }



        public static void SendMails()
        {
            //"mobilityacbsp@gmail.com"
            //"1234qawsed";
            MailMessage confirmationMail = PrepareMail(true);
            MailMessage applicationMail = PrepareMail(false);

            NetworkCredential networkCredential = new NetworkCredential(From, Password);

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

                mail.Subject = ApplicationMailSubject;
                string pathFile = Model.FacultySelected ? @"~/App_Data/MailBodyApplicationFaculty.txt" : @"~/App_Data/MailBodyApplicationStudent.txt";
                var fileContents = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(pathFile));
                mail.IsBodyHtml = false;
                mail.Body = Model.FacultySelected ? SetUpBodyApplicationFaculty(fileContents) : SetUpBodyApplicationStudent(fileContents);

                Attachment messageAttachment = new Attachment(Model.CV.InputStream, Model.CV.FileName);
                mail.Attachments.Add(messageAttachment);

                if (Model.Transcript != null)
                {
                    Attachment transcript = new Attachment(Model.Transcript.InputStream, Model.Transcript.FileName);
                    mail.Attachments.Add(transcript);
                }
            }
            else
            {
                mail.Subject = ConfirmationMailSubject;
                string pathFile = Model.FacultySelected ? @"~/App_Data/MailBodyConfirmationFaculty.txt" : @"~/App_Data/MailBodyConfirmationStudent.txt";
                var fileContents = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(pathFile));
                mail.Body = Model.FacultySelected ? SetUpBodyConfirmationFaculty(fileContents) : SetUpBodyConfirmationStudent(fileContents);
                mail.IsBodyHtml = false;
            }

            return mail;
        }

        private static string SetUpBodyApplicationFaculty(string content)
        {
            content = content.Replace("(Gender)", (Model.Gender == "Male" ? "Mr." : "Ms.")).Replace("(FirstName)", Model.FirstName)
                             .Replace("(LastName)", Model.LastName).Replace("(CountryOfOrigin)", Model.CountryOfOrigin)
                             .Replace("(Nationality)", Model.Nationality).Replace("(EnglishLevel)", Model.EnglishLevel)
                             .Replace("(UniversityFrom)", Model.UniversityFrom).Replace("(EmailAddress)", Model.Email)
                             .Replace("(HighestDegree)", Model.HighestDegree)
                             .Replace("(FirstCourse)", Model.FirstCourseName).Replace("(SecondCourse)", Model.SecondCourseName)
                             .Replace("(ThirdCourse)", Model.ThirdCourseName).Replace("(FourthCourse)", Model.FourthCourseName);

            //FacultyDegreeLevel i DegreeLevel nema
            return content;

        }

        private static string SetUpBodyApplicationStudent(string content)
        {
            content = content.Replace("(Gender)", (Model.Gender == "Male" ? "Mr." : "Ms."))
                             .Replace("(FirstName)", Model.FirstName)
                             .Replace("(LastName)", Model.LastName)
                             .Replace("(CountryOfOrigin)", Model.CountryOfOrigin)
                             .Replace("(Nationality)", Model.Nationality)
                             .Replace("(EnglishLevel)", Model.EnglishLevel)
                             .Replace("(ProgramEnrolled)", Model.ProgramEnrolled)
                             .Replace("(ProgramEnrolled)", Model.ProgramEnrolled)
                             .Replace("(YearOfCompletion)", Model.YearOfCompletion)
                             .Replace("(YearOfEnrollment)", Model.YearOfCompletion)
                             .Replace("(SemesterOfEnrollment)", Model.SemesterEnrolled)
                             .Replace("(UniversityFrom)", Model.UniversityFrom)
                             .Replace("(EmailAddress)", Model.Email);

            // (UniversityCountry)

            return content;

        }

        private static string SetUpBodyConfirmationFaculty(string content)
        {
            content = content.Replace("(Gender)", (Model.Gender == "Male" ? "Mr." : "Ms."))
                             .Replace("(FirstName)", Model.FirstName)
                             .Replace("(LastName)", Model.LastName)
                             .Replace("(ProgramEnrolled)", Model.Faculty.Program)
                             .Replace("(SemesterOfEnrollment)", Model.SemesterEnrolled)
                             .Replace("(UniversityCountry)", Model.Faculty.CountryObj.Name);

            return content;

        }

        private static string SetUpBodyConfirmationStudent(string content)
        {
            content = content.Replace("(Gender)", (Model.Gender == "Male" ? "Mr." : "Ms."))
                             .Replace("(FirstName)", Model.FirstName)
                             .Replace("(LastName)", Model.LastName)
                             .Replace("(ProgramEnrolled)", Model.Faculty.Program)
                             .Replace("(SemesterOfEnrollment)", Model.SemesterEnrolled)
                             .Replace("(UniversityCountry)", Model.Faculty.CountryObj.Name);
            return content;

        }
        private static SmtpClient GetSmtp(NetworkCredential networkCredential)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Host;
            smtp.EnableSsl = EnableSSL;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = Port;
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
                lookup.Timeout = TimeSpan.FromSeconds(25);
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