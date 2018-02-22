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
    public class NomDegreeLevelsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomDegreeLevels
        public ActionResult Index()
        {
            var nomDegreeLevels = db.DegreeLevels.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomDegreeLevels.ToList());
        }

        // GET: NomDegreeLevels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomDegreeLevel nomDegreeLevel = db.DegreeLevels.Find(id);
            if (nomDegreeLevel == null)
            {
                return HttpNotFound();
            }
            return View(nomDegreeLevel);
        }

        // GET: NomDegreeLevels/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: NomDegreeLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] NomDegreeLevel nomDegreeLevel)
        {
            NomDegreeLevel degreeLevel = new NomDegreeLevel();
            degreeLevel.Name = nomDegreeLevel.Name;

            if (ModelState.IsValid)
            {
                db.DegreeLevels.Add(degreeLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomDegreeLevel.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomDegreeLevel.RegisteredBy);
            return View(nomDegreeLevel);
        }

        // GET: NomDegreeLevels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomDegreeLevel nomDegreeLevel = db.DegreeLevels.Find(id);
            if (nomDegreeLevel == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomDegreeLevel.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomDegreeLevel.RegisteredBy);
            return View(nomDegreeLevel);
        }

        // POST: NomDegreeLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] NomDegreeLevel nomDegreeLevel)
        {
            NomDegreeLevel degreeLevelDb = db.DegreeLevels.Find(nomDegreeLevel.ID);
            degreeLevelDb.Name = nomDegreeLevel.Name;

            if (ModelState.IsValid)
            {
                db.Entry(degreeLevelDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomDegreeLevel.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomDegreeLevel.RegisteredBy);
            return View(nomDegreeLevel);
        }

        // GET: NomDegreeLevels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomDegreeLevel nomDegreeLevel = db.DegreeLevels.Find(id);
            if (nomDegreeLevel == null)
            {
                return HttpNotFound();
            }
            return View(nomDegreeLevel);
        }

        // POST: NomDegreeLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NomDegreeLevel nomDegreeLevel = db.DegreeLevels.Find(id);
            db.DegreeLevels.Remove(nomDegreeLevel);
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
