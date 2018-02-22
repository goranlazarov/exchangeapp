using ExchangeApp.Models;
using System;
using System.Web.Mvc;

namespace ExchangeApp.Controllers
{
    public class TeacherController : BaseController
    {

        // GET: Faculties/Create
        public ActionResult Index()
        {
            ViewBag.CountryOfOrigin = new SelectList(db.Countries, "ID", "Name");
            ViewBag.ApplicantHighestDegree = new SelectList(db.Countries, "ID", "Name");
            ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "ID", "Name");
            ViewBag.SchoolName = new SelectList(db.Faculties, "ID", "Name");
            ViewBag.Country = new SelectList(db.Countries, "ID", "Name");
            ViewBag.Region = new SelectList(db.Regions, "ID", "Name");
            ViewBag.FirstSubject = new SelectList(db.Subjects, "ID", "Name");
            ViewBag.SecondSubject = new SelectList(db.Subjects, "ID", "Name");
            ViewBag.ThirdSubject = new SelectList(db.Subjects, "ID", "Name");
            ViewBag.FourthSubject = new SelectList(db.Subjects, "ID", "Name");

            return View();
        }
     
    }
}