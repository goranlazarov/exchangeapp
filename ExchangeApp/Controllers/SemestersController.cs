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
    public class SemestersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Semesters
        public ActionResult Index()
        {
            var semesters = db.Semesters.Include(s => s.LastUpdatedByUser).Include(s => s.RegisteredByUser).Include(s => s.SchoolYearObj);
            return View(semesters.ToList());
        }

        // GET: Semesters/Details/5
        public ActionResult Details(int? id)
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

        // GET: Semesters/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.SchoolYearId = new SelectList(db.SchoolYears, "ID", "Name");
            return View();
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,SchoolYearId")] Semester semester)
        {
            Semester semesterToCreate = new Semester();
            semesterToCreate.Description = semester.Description;
            semesterToCreate.SchoolYearId = semester.SchoolYearId;

            if (ModelState.IsValid)
            {
                db.Semesters.Add(semesterToCreate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", semester.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", semester.RegisteredBy);
            ViewBag.SchoolYearId = new SelectList(db.SchoolYears, "ID", "Name", semester.SchoolYearId);
            return View(semester);
        }

        // GET: Semesters/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", semester.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", semester.RegisteredBy);
            ViewBag.SchoolYearId = new SelectList(db.SchoolYears, "ID", "Name", semester.SchoolYearId);
            return View(semester);
        }

        // POST: Semesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,SchoolYearId,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] Semester semester)
        {
            Semester semesterDb = db.Semesters.Find(semester.ID);
            semesterDb.Description = semester.Description;
            semesterDb.SchoolYearId = semester.SchoolYearId;

            if (ModelState.IsValid)
            {
                db.Entry(semesterDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", semester.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", semester.RegisteredBy);
            ViewBag.SchoolYearId = new SelectList(db.SchoolYears, "ID", "Name", semester.SchoolYearId);
            return View(semester);
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
    }
}
