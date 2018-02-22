using ExchangeApp.Models;
using System;
using System.Web.Mvc;

namespace ExchangeApp.Controllers
{
    public class StudentController : BaseController
    {

        // GET: Faculties/Create
        public ActionResult Index()
        {
            ViewBag.CountryOfOrigin = new SelectList(db.Countries, "ID", "Name");
            ViewBag.EnglishLevel = new SelectList(db.EnglishLevels, "ID", "Name");
            ViewBag.SchoolName = new SelectList(db.Faculties, "ID", "Name");
            ViewBag.Country = new SelectList(db.Countries, "ID", "Name");
            ViewBag.Region = new SelectList(db.Regions, "ID", "Name");

            return View();
        }
     
    }
}