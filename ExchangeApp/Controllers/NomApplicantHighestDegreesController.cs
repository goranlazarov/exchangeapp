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
    public class NomApplicantHighestDegreesController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomApplicantHighestDegrees
        public ActionResult Index()
        {
            var nomApplicantHighestDegrees = db.ApplicantHighestDegrees.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomApplicantHighestDegrees.ToList());
        }

        // GET: NomApplicantHighestDegrees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomApplicantHighestDegree nomApplicantHighestDegree = db.ApplicantHighestDegrees.Find(id);
            if (nomApplicantHighestDegree == null)
            {
                return HttpNotFound();
            }
            return View(nomApplicantHighestDegree);
        }

        // GET: NomApplicantHighestDegrees/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: NomApplicantHighestDegrees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] NomApplicantHighestDegree nomApplicantHighestDegree)
        {
            NomApplicantHighestDegree highestDegree = new NomApplicantHighestDegree();
            highestDegree.Name = nomApplicantHighestDegree.Name;

            if (ModelState.IsValid)
            {
                db.ApplicantHighestDegrees.Add(highestDegree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomApplicantHighestDegree.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomApplicantHighestDegree.RegisteredBy);
            return View(nomApplicantHighestDegree);
        }

        // GET: NomApplicantHighestDegrees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomApplicantHighestDegree nomApplicantHighestDegree = db.ApplicantHighestDegrees.Find(id);
            if (nomApplicantHighestDegree == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomApplicantHighestDegree.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomApplicantHighestDegree.RegisteredBy);
            return View(nomApplicantHighestDegree);
        }

        // POST: NomApplicantHighestDegrees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] NomApplicantHighestDegree nomApplicantHighestDegree)
        {
            NomApplicantHighestDegree highestDegreeDb = db.ApplicantHighestDegrees.Find(nomApplicantHighestDegree.ID);
            highestDegreeDb.Name = nomApplicantHighestDegree.Name;

            if (ModelState.IsValid)
            {
                db.Entry(highestDegreeDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomApplicantHighestDegree.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomApplicantHighestDegree.RegisteredBy);
            return View(nomApplicantHighestDegree);
        }

        // GET: NomApplicantHighestDegrees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomApplicantHighestDegree nomApplicantHighestDegree = db.ApplicantHighestDegrees.Find(id);
            if (nomApplicantHighestDegree == null)
            {
                return HttpNotFound();
            }
            return View(nomApplicantHighestDegree);
        }

        // POST: NomApplicantHighestDegrees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NomApplicantHighestDegree nomApplicantHighestDegree = db.ApplicantHighestDegrees.Find(id);
            db.ApplicantHighestDegrees.Remove(nomApplicantHighestDegree);
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
