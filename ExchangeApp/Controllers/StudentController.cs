using DnsClient;
using ExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using ExchangeApp.Helpers;

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
            initialC.Name = "Select country/state";
            List<Models.NomCountry> listCountries = new List<Models.NomCountry>();
            listCountries.Add(initialC);
            listCountries.AddRange(db.Countries.OrderBy(x => x.Name));
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
            FacultyViewModel facultyViewModel = new FacultyViewModel(faculty);
            if (faculty == null)
            {
                return HttpNotFound();
            }

            ViewBag.CountryOfOrigin = new SelectList(db.Countries.OrderBy(x => x.Name), "Name", "Name");
            ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "Name", "Name");
            ViewBag.YearOfEnrollment = new SelectList(db.SchoolYears, "Name", "Name");
            ViewBag.YearOfCompletion = new SelectList(db.SchoolYears, "Name", "Name");
            ViewBag.HighestDegrees = new SelectList(db.ApplicantHighestDegrees, "Name", "Name");
        

            Subject initialSubject = new Subject();
            initialSubject.ID = -1;
            initialSubject.Name = "Select course";
            List<Subject> subjects = new List<Subject>();
            subjects.Add(initialSubject);
            foreach (var course in faculty.Courses)
            {
                subjects.Add(course.SubjectObj);
            }

            ViewBag.FacultyCourses = new SelectList(subjects, "ID", "Name");
            AddSearchFields();

            StudentViewModel svm = new StudentViewModel(facultyViewModel);
            return View(svm);
        }

        [HttpPost]
        public ActionResult SendApplication(StudentViewModel model)
        {

            var logger = NLog.LogManager.GetCurrentClassLogger();

            AddSearchFields();
            Faculty faculty = db.Faculties.Find(model.Faculty.ID);
            FacultyViewModel facultyViewModel = new FacultyViewModel(faculty);
            ValidateApplication(faculty, model);


            ViewBag.CountryOfOrigin = new SelectList(db.Countries.OrderBy(x => x.Name), "Name", "Name");
            ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "Name", "Name");
            ViewBag.YearOfEnrollment = new SelectList(db.SchoolYears, "Name", "Name");
            ViewBag.YearOfCompletion = new SelectList(db.SchoolYears, "Name", "Name");
            ViewBag.HighestDegrees = new SelectList(db.ApplicantHighestDegrees, "Name", "Name");
          
      

            Subject initialSubject = new Subject();
            initialSubject.ID = -1;
            initialSubject.Name = "Select course";
            List<Subject> subjects = new List<Subject>();
            subjects.Add(initialSubject);
            foreach (var course in faculty.Courses)
            {
                subjects.Add(course.SubjectObj);
            }
            ViewBag.FacultyCourses = new SelectList(subjects, "ID", "Name");

            StudentViewModel svm = new StudentViewModel(facultyViewModel);

            if (ModelState.IsValid)
            {
                model.Faculty = facultyViewModel;
                MailSender.Model = model;
                //var task = MailSender.IsValidAsync();
                //task.Wait();
                //bool isValidAsync = task.Result;

                if (MailSender.IsValidEmail() && model.CV != null && model.CV.ContentLength > 0 )
                {

                    try
                    {
                        MailSender.SendMails();

                        DisplaySuccessMessage("Successfully sent mail for application!");
                        logger.Info("Successfully sent mail for application!");
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception e)
                    {
                        DisplayErrorMessage("An error occurred while sending mail for application!");
                        logger.Info("Error while sending -> " + e.Message + " ,other info -> " + e.InnerException);

                    }
                }

                DisplayErrorMessage("An error occurred while sending mail for application!");
            }

            return View("Index", svm);

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

            if (model.UniversityFrom == null)
            {
                ModelState.AddModelError("UniversityFrom", "University from is required ");
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

                if (model.YearOfEnrollment == null)
                {
                    ModelState.AddModelError("YearOfEnrollment", "Please choose year of enrollment ");
                }

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

                if (model.FirstCourse == "-1" && model.SecondCourse == "-1" && model.ThirdCourse == "-1" && model.FourthCourse == "-1")
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