using DnsClient;
using ExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace ExchangeApp.Controllers
{
    public class StudentController : BaseController
    {

        protected void AddSearchFields()
        {
            NomRegion initial = new NomRegion();
            initial.ID = -1;
            initial.Name = "Select region";
            List<Models.NomRegion> list = new List<Models.NomRegion>();
            list.Add(initial);
            list.AddRange(db.Regions);

            NomCountry initialC = new NomCountry();
            initialC.ID = -1;
            initialC.Name = "Select country";
            List<Models.NomCountry> listCountries = new List<Models.NomCountry>();
            listCountries.Add(initialC);
            listCountries.AddRange(db.Countries);
            ViewBag.RegionId = new SelectList(list, "ID", "Name");
            ViewBag.CountryId = new SelectList(listCountries, "ID", "Name");
        }

        // GET: Faculties/Create
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }

            ViewBag.CountryOfOrigin = new SelectList(db.Countries, "ID", "Name");
            ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "ID", "Name");

            AddSearchFields();

            StudentViewModel svm = new StudentViewModel(faculty);
            svm.DateOfEnrollment = DateTime.Today;
            svm.DateOfCompletion = DateTime.Today;

            return View(svm);
        }

        [HttpPost]
        public ActionResult SendApplication(StudentViewModel model)
        {
            AddSearchFields();

            ViewBag.CountryOfOrigin = new SelectList(db.Countries, "ID", "Name");
            ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "ID", "Name");

            Faculty faculty = db.Faculties.Find(model.Faculty.ID);
            StudentViewModel svm = new StudentViewModel(faculty);

            var task = IsValidAsync(model.Email);
            task.Wait();
            bool isValidAsync = task.Result;

            if (isValidAsync && IsValidEmail(model.Email) && model.CV != null && model.CV.ContentLength > 0)
            {

                try
                {
                    SendMails(model);

                    DisplaySuccessMessage("Successfully sent mail for application!");

                    return View("Index", svm);
                }
                catch(Exception e)
                {
                    DisplayErrorMessage("An error occurred while sending mail for application!");
                    
                    return View("Index", svm);
                }
                
            }

            DisplayErrorMessage("An error occurred while sending mail for application!");

            return View("Index", svm);

        }

        private void SendMails(StudentViewModel model)
        {
            string from = "glazarov9@gmail.com"; //"mobilityacbsp@gmail.com"; //model.Email;
            string password = "brooklyn78";//"1234qawsed";
            string ccMail = "goran_sr@hotmail.com";

            MailMessage confirmationMail =  PrepareMail(model, from, null, true);
            MailMessage applicationMail = PrepareMail(model, from, ccMail, false);

            NetworkCredential networkCredential = new NetworkCredential(from, password);

            GetSmtp(networkCredential).Send(confirmationMail);
            GetSmtp(networkCredential).Send(applicationMail);
            



        }

        private MailMessage PrepareMail(StudentViewModel model, string from, string ccMail, bool isConfirmation)
        {
            
            MailMessage mail = new MailMessage(from, ( !isConfirmation ? model.Faculty.Email : model.Email));
            if (!isConfirmation)
            {
                MailAddress ccMailAddress = new MailAddress(ccMail);
                mail.CC.Add(ccMailAddress);
               
                mail.Subject = "ACBSP mobility application from";

                var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/MailBodyApplication.txt"));
                mail.Body = fileContents.Replace("FirstName", model.FirstName).Replace("LastName", model.LastName);
                mail.IsBodyHtml = false;

                Attachment messageAttachment = new Attachment(model.CV.InputStream, model.CV.FileName);
                mail.Attachments.Add(messageAttachment);
            }
            else
            {
                mail.Subject = "ACBSP mobility confirmation for successfull application";

                var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/MailBodyConfirmation.txt"));
                mail.IsBodyHtml = false;
            }

            return mail;
        }

        private SmtpClient GetSmtp(NetworkCredential networkCredential)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            return smtp;
        }

        Task<bool> IsValidAsync(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                var host = mailAddress.Host;
                return CheckDnsEntriesAsync(host);
            }
            catch (FormatException)
            {
                return Task.FromResult(false);
            }
        }

        async Task<bool> CheckDnsEntriesAsync(string domain)
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

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


    }
}