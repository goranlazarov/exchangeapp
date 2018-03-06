using ExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

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
            //ViewBag.Faculty = db.Faculties.Find(1);

            //ViewBag.CountryOfOrigin = new SelectList(db.Countries, "ID", "Name");
            //ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "ID", "Name");
            //ViewBag.SchoolName = new SelectList(db.Faculties, "ID", "Name");
            //ViewBag.Country = new SelectList(db.Countries, "ID", "Name");
            //ViewBag.Region = new SelectList(db.Regions, "ID", "Name");
            //return View();

            

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
            return View(svm);
        }

        [HttpPost]
        public ActionResult SendApplication(StudentViewModel model)
        {
            if (model.CV !=null && model.CV.ContentLength > 0)
            {
                Attachment messageAttachment = new Attachment(model.CV.InputStream, model.CV.FileName);
                string from = model.Email;
                string password = model.Password;
                using (MailMessage mail = new MailMessage(from, model.Faculty.Email))
                {

                    //da se zema od forma
                    mail.Subject = String.Format("Application for Student exchange ");
                    //read from file
                    var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/MailBody.txt"));
                    mail.Body = fileContents.Replace("FirstName", model.FirstName).Replace("LastName", model.LastName);
                    mail.Attachments.Add(messageAttachment);
                    mail.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    //student mail
                    NetworkCredential networkCredential = new NetworkCredential(from, password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mail);

                    DisplaySuccessMessage("Successfully sent mail for application!");

                    AddSearchFields();
                    ViewBag.CountryOfOrigin = new SelectList(db.Countries, "ID", "Name");
                    ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "ID", "Name");
                    Faculty faculty = db.Faculties.Find(model.Faculty.ID);
                    StudentViewModel svm = new StudentViewModel(faculty);
                    return View("Index", svm);

                }
            }

            DisplayErrorMessage("An error occurred while sending mail for application!");

            AddSearchFields();
            ViewBag.CountryOfOrigin = new SelectList(db.Countries, "ID", "Name");
            ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "ID", "Name");
            Faculty facultyOth = db.Faculties.Find(model.Faculty.ID);
            StudentViewModel svmOth = new StudentViewModel(facultyOth);
            return View("Index", svmOth);
            
        }


    }
}