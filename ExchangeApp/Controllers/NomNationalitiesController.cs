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
    public class NomNationalitiesController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomNationalities
        public ActionResult Index()
        {
            var nomNationalities = db.Nationalities.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomNationalities.ToList());
        }

        public ActionResult AddEditNationality(int nationalityId)
        {
            NomNationality model = new NomNationality();

            if (nationalityId > 0)
            {
                NomNationality nationality = db.Nationalities.Find(nationalityId);
                model.ID = nationality.ID;
                model.Name = nationality.Name;
            }

            return PartialView("AddEditNationality", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateNationality(NomNationality model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (model.ID > 0)
                    {
                        NomNationality nationalityDb = db.Nationalities.FirstOrDefault(x => x.ID == model.ID);
                        nationalityDb.ID = model.ID;
                        nationalityDb.Name = model.Name;

                        message = "Successfully edited nationality!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.Nationalities.Any(x => x.Name.ToLower() == model.Name.ToLower()))
                        {
                            throw new Exception("Nationality already exists!");
                        }

                        NomNationality nationality = new NomNationality();
                        nationality.ID = model.ID;
                        nationality.Name = model.Name;

                        message = "Successfully added nationality!";

                        db.Nationalities.Add(nationality);
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
        public ActionResult DeleteNationality(int id)
        {
            NomNationality nationalityDb = db.Nationalities.Find(id);
            db.Nationalities.Remove(nationalityDb);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted nationality!");
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
