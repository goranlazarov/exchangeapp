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

        // GET: Semesters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Semester semester = db.Semesters.Find(id);
            db.Semesters.Remove(semester);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
                    List<NomSchoolYear> sy = db.SchoolYears.ToList();
                    ViewBag.SchoolYearsList = new SelectList(sy, "ID", "Name");

                    if (model.ID > 0)
                    {
                        Semester semesterDb = db.Semesters.FirstOrDefault(x => x.ID == model.ID);
                        semesterDb.ID = model.ID;
                        semesterDb.Description = model.Description;

                        NomSchoolYear schoolYear = db.SchoolYears.Find(model.SchoolYearId);
                        semesterDb.SchoolYearObj = schoolYear;
                        semesterDb.SchoolYearId = schoolYear.ID;

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

                        db.Semesters.Add(semester);
                        db.SaveChanges();
                    }

                    return Json(true);
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                var modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }

                return Json(modelErrors);
            }

        }

    }
}
