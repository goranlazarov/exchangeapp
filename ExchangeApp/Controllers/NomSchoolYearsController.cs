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
    public class NomSchoolYearsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomSchoolYears
        public ActionResult Index()
        {
            var nomSchoolYears = db.SchoolYears.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomSchoolYears.ToList());
        }

        // GET: NomSchoolYears/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomSchoolYear nomSchoolYear = db.SchoolYears.Find(id);
            if (nomSchoolYear == null)
            {
                return HttpNotFound();
            }
            return View(nomSchoolYear);
        }

        // GET: NomSchoolYears/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: NomSchoolYears/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] NomSchoolYear nomSchoolYear)
        {
            NomSchoolYear schoolYear = new NomSchoolYear();
            schoolYear.Name = nomSchoolYear.Name;

            if (ModelState.IsValid)
            {
                db.SchoolYears.Add(schoolYear);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomSchoolYear.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomSchoolYear.RegisteredBy);
            return View(nomSchoolYear);
        }

        // GET: NomSchoolYears/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomSchoolYear nomSchoolYear = db.SchoolYears.Find(id);
            if (nomSchoolYear == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomSchoolYear.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomSchoolYear.RegisteredBy);
            return View(nomSchoolYear);
        }

        // POST: NomSchoolYears/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] NomSchoolYear nomSchoolYear)
        {
            NomSchoolYear schoolYearDb = db.SchoolYears.Find(nomSchoolYear.ID);
            schoolYearDb.Name = nomSchoolYear.Name;

            if (ModelState.IsValid)
            {
                db.Entry(schoolYearDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomSchoolYear.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomSchoolYear.RegisteredBy);
            return View(nomSchoolYear);
        }

        // GET: NomSchoolYears/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomSchoolYear nomSchoolYear = db.SchoolYears.Find(id);
            if (nomSchoolYear == null)
            {
                return HttpNotFound();
            }
            return View(nomSchoolYear);
        }

        // POST: NomSchoolYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NomSchoolYear nomSchoolYear = db.SchoolYears.Find(id);
            db.SchoolYears.Remove(nomSchoolYear);
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
