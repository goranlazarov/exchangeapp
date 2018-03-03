using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExchangeApp.Models;

namespace ExchangeApp.Controllers
{
    public class NomEnglishLevelsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomEnglishLevels
        public ActionResult Index()
        {
            var nomEnglishLevels = db.EnglishLevels.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomEnglishLevels.ToList());
        }

        public ActionResult AddEditEnglishLevel(int englishLevelId)
        {
            NomEnglishLevel model = new NomEnglishLevel();

            if (englishLevelId > 0)
            {
                NomEnglishLevel englishLevel = db.EnglishLevels.Find(englishLevelId);
                model.ID = englishLevel.ID;
                model.Name = englishLevel.Name;
            }

            return PartialView("AddEditEnglishLevel", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateEnglishLevel(NomEnglishLevel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (db.EnglishLevels.Any(x => x.Name.ToLower() == model.Name.ToLower()))
                    {
                        throw new Exception("English level already exists!");
                    }

                    if (model.ID > 0)
                    {
                        NomEnglishLevel englishLevelDb = db.EnglishLevels.FirstOrDefault(x => x.ID == model.ID);
                        englishLevelDb.ID = model.ID;
                        englishLevelDb.Name = model.Name;

                        message = "Successfully edited english level!";

                        db.SaveChanges();
                    }
                    else
                    {
                        NomEnglishLevel englishLevel = new NomEnglishLevel();

                        englishLevel.ID = model.ID;
                        englishLevel.Name = model.Name;

                        message = "Successfully added english level!";

                        db.EnglishLevels.Add(englishLevel);
                        db.SaveChanges();
                    }

                    DisplaySuccessMessage(message);
                    return Json(true);
                }

                catch (Exception ex)
                {
                    var modelErrors = new List<string>();
                    modelErrors.Add(ex.Message);

                    return Json(modelErrors);
                }
            }
            else
            {
                var errors = GetModelStateErrors(ModelState.Values);
                return Json(errors);
            }

        }

        [HttpPost]
        public ActionResult DeleteEnglishLevel(int id)
        {
            NomEnglishLevel englishLevelDb = db.EnglishLevels.Find(id);
            db.EnglishLevels.Remove(englishLevelDb);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted english level!");
            return Json(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
