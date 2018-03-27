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
            ViewBag.YearOfEnrollment = new SelectList(db.SchoolYears, "ID", "Name");
            ViewBag.YearOfCompletion = new SelectList(db.SchoolYears, "ID", "Name");
            ViewBag.HighestDegrees = new SelectList(db.ApplicantHighestDegrees, "ID", "Name");
            ViewBag.FacultyCourses = new SelectList(faculty.Subjects, "ID", "Name");

            AddSearchFields();

            StudentViewModel svm = new StudentViewModel(faculty);
            return View(svm);
        }

        [HttpPost]
        public ActionResult SendApplication(StudentViewModel model)
        {
            AddSearchFields();
            Faculty faculty = db.Faculties.Find(model.Faculty.ID);

            ValidateApplication(faculty, model);

            ViewBag.CountryOfOrigin = new SelectList(db.Countries, "ID", "Name");
            ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "ID", "Name");
            ViewBag.YearOfEnrollment = new SelectList(db.SchoolYears, "ID", "Name");
            ViewBag.YearOfCompletion = new SelectList(db.SchoolYears, "ID", "Name");
            ViewBag.HighestDegrees = new SelectList(db.ApplicantHighestDegrees, "ID", "Name");
            ViewBag.FacultyCourses = new SelectList(faculty.Subjects, "ID", "Name");

            StudentViewModel svm = new StudentViewModel(faculty);

           // if (ModelState.IsValid)
          //  {
                var task = IsValidAsync(model.Email);
                task.Wait();
                bool isValidAsync = task.Result;

                if (isValidAsync && IsValidEmail(model.Email) && model.CV != null && model.CV.ContentLength > 0)
                {

                    try
                    {
                       // SendMails(model);

                        DisplaySuccessMessage("Successfully sent mail for application!");

                        return View("Index", svm);
                    }
                    catch (Exception e)
                    {
                        DisplayErrorMessage("An error occurred while sending mail for application!");

                    }
                }

                DisplayErrorMessage("An error occurred while sending mail for application!");
           // }

            return View("Index", svm);

        }

        private void SendMails(StudentViewModel model)
        {
            string from = "glazarov9@gmail.com"; //"mobilityacbsp@gmail.com"; //model.Email;
            string password = "brooklyn78";//"1234qawsed";
            string ccMail = "goran_sr@hotmail.com";

            MailMessage confirmationMail = PrepareMail(model, from, null, true);
            MailMessage applicationMail = PrepareMail(model, from, ccMail, false);

            NetworkCredential networkCredential = new NetworkCredential(from, password);

            GetSmtp(networkCredential).Send(confirmationMail);
            GetSmtp(networkCredential).Send(applicationMail);
        }

        private MailMessage PrepareMail(StudentViewModel model, string from, string ccMail, bool isConfirmation)
        {

            MailMessage mail = new MailMessage(from, (!isConfirmation ? model.Faculty.Email : model.Email));
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

        private void ValidateApplication(Faculty faculty, StudentViewModel model)
        {

            if (faculty.FacultyTypeOfExchangeObj != null && faculty.StudentTypeOfExchangeObj != null)
            {
                if (!model.StudentSelected && !model.FacultySelected)
                {
                    ModelState.AddModelError("", "Please select application type");
                }
            }

            if (model.FirstName == null)
            {
                ModelState.AddModelError("FirstName", "First name is required ");
            }

            if (model.LastName == null)
            {
                ModelState.AddModelError("LastName", "Last name is required ");
            }

            if (model.Email == null)
            {
                ModelState.AddModelError("Email", "Email is required ");
            }

            if (model.CountryOfOrigin == null)
            {
                ModelState.AddModelError("CountryOfOrigin", "Please choose country ");
            }

            if (model.EnglishLevel == null)
            {
                ModelState.AddModelError("EnglishLevel", "Please choose english level ");
            }

            if (model.StudentSelected || (faculty.StudentTypeOfExchangeObj != null && faculty.FacultyTypeOfExchangeObj == null))
            {
                if (model.ProgramEnrolled == null)
                {
                    ModelState.AddModelError("ProgramEnrolled", "Program enrolled is required");
                }

                if (model.SemesterEnrolled == null)
                {
                    ModelState.AddModelError("SemesterEnrolled", "Semester enrolled is required");
                }

                //if (model.YearOfEnrollment == null)
                //{
                //    ModelState.AddModelError("YearOfEnrollment", "Please choose year of enrollment ");
                //}

                if (model.YearOfCompletion == null)
                {
                    ModelState.AddModelError("YearOfCompletion", "Please choose year of completion ");
                }
            }

            if (model.FacultySelected || (faculty.StudentTypeOfExchangeObj == null && faculty.FacultyTypeOfExchangeObj != null))
            {
                if (model.HighestDegree == null)
                {
                    ModelState.AddModelError("HighestDegree", "Please choose highest degree ");
                }

                if (model.FirstCourse == null && model.SecondCourse == null && model.ThirdCourse == null && model.FourthCourse == null)
                {
                    ModelState.AddModelError("FirstCourse", "Please choose at least one course");
                }
            }


            if (model.AgreementNumber == null || model.AgreementNumber.ToLower() != faculty.AgreementNumber.ToLower())
            {
                ModelState.AddModelError("AgreementNumber", "Invalid agreement number");
            }
        }


    }
}