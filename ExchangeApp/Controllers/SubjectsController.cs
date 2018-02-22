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
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subjects
        public ActionResult Index()
        {
            var subjects = db.Subjects.Include(s => s.DegreeLevelObj).Include(s => s.FacultyObj).Include(s => s.LastUpdatedByUser).Include(s => s.RegisteredByUser);
            return View(subjects.ToList());
        }

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            ViewBag.DegreeLevelId = new SelectList(db.DegreeLevels, "ID", "Name");
            ViewBag.FacultyId = new SelectList(db.Faculties, "ID", "Name");
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,FacultyId,DegreeLevelId")] Subject subject)
        {
            Subject newSubject = new Subject();
            newSubject.Name = subject.Name;
            newSubject.FacultyId = subject.FacultyId;
            newSubject.DegreeLevelId = subject.DegreeLevelId;

            if (ModelState.IsValid)
            {
                db.Subjects.Add(newSubject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DegreeLevelId = new SelectList(db.DegreeLevels, "ID", "Name", subject.DegreeLevelId);
            ViewBag.FacultyId = new SelectList(db.Faculties, "ID", "Name", subject.FacultyId);
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", subject.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", subject.RegisteredBy);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.DegreeLevelId = new SelectList(db.DegreeLevels, "ID", "Name", subject.DegreeLevelId);
            ViewBag.FacultyId = new SelectList(db.Faculties, "ID", "Name", subject.FacultyId);
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", subject.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", subject.RegisteredBy);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,FacultyId,DegreeLevelId,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] Subject subject)
        {
            Subject subjectDb = db.Subjects.Find(subject.ID);
            subjectDb.Name = subject.Name;
            subjectDb.FacultyId = subject.FacultyId;
            subjectDb.DegreeLevelId = subject.DegreeLevelId;

            if (ModelState.IsValid)
            {
                db.Entry(subjectDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DegreeLevelId = new SelectList(db.DegreeLevels, "ID", "Name", subject.DegreeLevelId);
            ViewBag.FacultyId = new SelectList(db.Faculties, "ID", "Name", subject.FacultyId);
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", subject.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", subject.RegisteredBy);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
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
    }
}
