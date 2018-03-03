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
    public class SemestersController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Semesters
        public ActionResult Index()
        {
            var semesters = db.Semesters.Include(s => s.LastUpdatedByUser).Include(s => s.RegisteredByUser).Include(s => s.SchoolYearObj);
            return View(semesters.ToList());
        }

        public ActionResult AddEditSemester(int semesterId)
        {
            List<NomSchoolYear> sy = db.SchoolYears.ToList();
            ViewBag.SchoolYearsList = new SelectList(sy, "ID", "Name");

            Semester model = new Semester();

            if (semesterId > 0)
            {
                Semester semester = db.Semesters.Find(semesterId);
                model.ID = semester.ID;
                model.Description = semester.Description;
                model.SchoolYearId = semester.SchoolYearId;
                model.SchoolYearObj = semester.SchoolYearObj;
            }

            return PartialView("AddEditSemester", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateSemester(Semester model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    List<NomSchoolYear> sy = db.SchoolYears.ToList();
                    ViewBag.SchoolYearsList = new SelectList(sy, "ID", "Name");

                    if (db.Semesters.Any(x => x.Description.ToLower() == model.Description.ToLower() &&
                                              x.SchoolYearId == model.SchoolYearId))
                    {
                        throw new Exception("Semester for that school year already exists!");
                    }

                    if (model.ID > 0)
                    {
                        Semester semesterDb = db.Semesters.FirstOrDefault(x => x.ID == model.ID);
                        semesterDb.ID = model.ID;
                        semesterDb.Description = model.Description;

                        NomSchoolYear schoolYear = db.SchoolYears.Find(model.SchoolYearId);
                        semesterDb.SchoolYearObj = schoolYear;
                        semesterDb.SchoolYearId = schoolYear.ID;
                        message = "Successfully edited semester!";

                        db.SaveChanges();
                    }
                    else
                    {
                        Semester semester = new Semester();

                        semester.ID = model.ID;
                        semester.Description = model.Description;

                        NomSchoolYear schoolYear = db.SchoolYears.Find(model.SchoolYearId);
                        semester.SchoolYearObj = schoolYear;
                        semester.SchoolYearId = schoolYear.ID;
                        message = "Successfully added semester!";

                        db.Semesters.Add(semester);
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
        public ActionResult DeleteSemester(int id)
        {
            Semester semester = db.Semesters.Find(id);
            db.Semesters.Remove(semester);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted semester!");
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
