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
    public class NomSchoolYearsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomSchoolYears
        public ActionResult Index()
        {
            var nomSchoolYears = db.SchoolYears.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomSchoolYears.ToList());
        }

        public ActionResult AddEditSchoolYear(int schoolYearId)
        {
            NomSchoolYear model = new NomSchoolYear();

            if (schoolYearId > 0)
            {
                NomSchoolYear schoolYear = db.SchoolYears.Find(schoolYearId);
                model.ID = schoolYear.ID;
                model.Name = schoolYear.Name;
            }

            return PartialView("AddEditSchoolYear", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateSchoolYear(NomSchoolYear model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (model.ID > 0)
                    {
                        NomSchoolYear schoolYearDb = db.SchoolYears.FirstOrDefault(x => x.ID == model.ID);
                        schoolYearDb.ID = model.ID;
                        schoolYearDb.Name = model.Name;

                        message = "Successfully edited school year!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.SchoolYears.Any(x => x.Name.ToLower() == model.Name.ToLower()))
                        {
                            throw new Exception("School year already exists!");
                        }

                        NomSchoolYear schoolYear = new NomSchoolYear();
                        schoolYear.ID = model.ID;
                        schoolYear.Name = model.Name;

                        message = "Successfully added school year!";

                        db.SchoolYears.Add(schoolYear);
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
        public ActionResult DeleteSchoolYear(int id)
        {
            NomSchoolYear schoolYearDb = db.SchoolYears.Find(id);
            db.SchoolYears.Remove(schoolYearDb);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted school year!");
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
