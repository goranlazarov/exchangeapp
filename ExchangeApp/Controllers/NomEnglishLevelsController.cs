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
    public class NomEnglishLevelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomEnglishLevels
        public ActionResult Index()
        {
            var nomEnglishLevels = db.EnglishLevels.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomEnglishLevels.ToList());
        }

        // GET: NomEnglishLevels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomEnglishLevel nomEnglishLevel = db.EnglishLevels.Find(id);
            if (nomEnglishLevel == null)
            {
                return HttpNotFound();
            }
            return View(nomEnglishLevel);
        }

        // GET: NomEnglishLevels/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: NomEnglishLevels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] NomEnglishLevel nomEnglishLevel)
        {
            NomEnglishLevel englishLevel = new NomEnglishLevel();
            englishLevel.Name = nomEnglishLevel.Name;

            if (ModelState.IsValid)
            {
                db.EnglishLevels.Add(englishLevel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomEnglishLevel.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomEnglishLevel.RegisteredBy);
            return View(nomEnglishLevel);
        }

        // GET: NomEnglishLevels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomEnglishLevel nomEnglishLevel = db.EnglishLevels.Find(id);
            if (nomEnglishLevel == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomEnglishLevel.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomEnglishLevel.RegisteredBy);
            return View(nomEnglishLevel);
        }

        // POST: NomEnglishLevels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] NomEnglishLevel nomEnglishLevel)
        {
            NomEnglishLevel englishLevelDb = db.EnglishLevels.Find(nomEnglishLevel.ID);
            englishLevelDb.Name = nomEnglishLevel.Name;

            if (ModelState.IsValid)
            {
                db.Entry(englishLevelDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomEnglishLevel.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomEnglishLevel.RegisteredBy);
            return View(nomEnglishLevel);
        }

        // GET: NomEnglishLevels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomEnglishLevel nomEnglishLevel = db.EnglishLevels.Find(id);
            if (nomEnglishLevel == null)
            {
                return HttpNotFound();
            }
            return View(nomEnglishLevel);
        }

        // POST: NomEnglishLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NomEnglishLevel nomEnglishLevel = db.EnglishLevels.Find(id);
            db.EnglishLevels.Remove(nomEnglishLevel);
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
