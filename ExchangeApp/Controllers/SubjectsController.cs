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
    public class SubjectsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subjects
        public ActionResult Index(int? page)
        {
            var subjects = db.Subjects.Include(s => s.FacultyObj).Include(s => s.LastUpdatedByUser).Include(s => s.RegisteredByUser);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(subjects.OrderBy(l => l.Name).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddEditSubject(int subjectId)
        {
            List<NomDegreeLevel> degreeLevels = db.DegreeLevels.ToList();
            ViewBag.DegreeLevelsList = new SelectList(degreeLevels, "ID", "Name");

            List<Faculty> facultiesList = db.Faculties.Where(x=>x.FacultyTypeOfExchangeObj != null).ToList();
            ViewBag.FacultiesList = new SelectList(facultiesList, "ID", "Name");

            Subject model = new Subject();

            if (subjectId > 0)
            {
                Subject subject = db.Subjects.Find(subjectId);
                model.ID = subject.ID;
                model.Name = subject.Name;
                model.FacultyId = subject.FacultyId;
            }

            return PartialView("AddEditSubject", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateSubject(Subject model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (model.ID > 0)
                    {
                        Subject subjectDb = db.Subjects.FirstOrDefault(x => x.ID == model.ID);
                        subjectDb.ID = model.ID;
                        subjectDb.Name = model.Name;
                        subjectDb.FacultyObj = model.FacultyObj;
                        subjectDb.FacultyId = model.FacultyId;

                        message = "Successfully edited subject!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.Subjects.Any(x => x.Name.ToLower() == model.Name.ToLower() && x.FacultyId == model.FacultyId))
                        {
                            throw new Exception("Subject for that faculty already exists!");
                        }

                        Subject subject = new Subject();
                        subject.ID = model.ID;
                        subject.Name = model.Name;

                        Faculty faculty = db.Faculties.Find(model.FacultyId);
                        subject.FacultyObj = faculty;
                        subject.FacultyId = faculty.ID;

                        message = "Successfully added subject!";

                        db.Subjects.Add(subject);
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
        public ActionResult DeleteSubject(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted subject!");
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
