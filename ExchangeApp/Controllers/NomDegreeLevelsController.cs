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
    public class NomDegreeLevelsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomDegreeLevels
        public ActionResult Index()
        {
            var nomDegreeLevels = db.DegreeLevels.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomDegreeLevels.ToList());
        }

        public ActionResult AddEditDegreeLevel(int degreeLevelId)
        {
            NomDegreeLevel model = new NomDegreeLevel();

            if (degreeLevelId > 0)
            {
                NomDegreeLevel degreeLevel = db.DegreeLevels.Find(degreeLevelId);
                model.ID = degreeLevel.ID;
                model.Name = degreeLevel.Name;
            }

            return PartialView("AddEditDegreeLevel", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateDegreeLevel(NomDegreeLevel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (model.ID > 0)
                    {
                        NomDegreeLevel degreeLevelDb = db.DegreeLevels.FirstOrDefault(x => x.ID == model.ID);
                        degreeLevelDb.ID = model.ID;
                        degreeLevelDb.Name = model.Name;

                        message = "Successfully edited degree level!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.DegreeLevels.Any(x => x.Name.ToLower() == model.Name.ToLower()))
                        {
                            throw new Exception("Degree level already exists!");
                        }

                        NomDegreeLevel degreeLevel = new NomDegreeLevel();
                        degreeLevel.ID = model.ID;
                        degreeLevel.Name = model.Name;

                        message = "Successfully added degree level!";

                        db.DegreeLevels.Add(degreeLevel);
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
        public ActionResult DeleteDegreeLevel(int id)
        {
            NomDegreeLevel degreeLevelDb = db.DegreeLevels.Find(id);
            db.DegreeLevels.Remove(degreeLevelDb);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted degree level!");
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
