using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExchangeApp.Models;
using PagedList;

namespace ExchangeApp.Controllers
{
    public class SemestersController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Semesters
        public ActionResult Index(int? page)
        {
            var semesters = db.Semesters.Include(s => s.LastUpdatedByUser).Include(s => s.RegisteredByUser);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(semesters.OrderBy(l => l.Registered).ToPagedList(pageNumber, pageSize));
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

                    if (model.ID > 0)
                    {
                        Semester semesterDb = db.Semesters.FirstOrDefault(x => x.ID == model.ID);
                        semesterDb.ID = model.ID;
                        semesterDb.Description = model.Description;

                        message = "Successfully edited semester!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.Semesters.Any(x => x.Description.ToLower() == model.Description.ToLower()))
                        {
                            throw new Exception("Semester for that school year already exists!");
                        }

                        Semester semester = new Semester();
                        semester.ID = model.ID;
                        semester.Description = model.Description;
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
