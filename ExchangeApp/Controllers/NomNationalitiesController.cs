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
    public class NomNationalitiesController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomNationalities
        public ActionResult Index()
        {
            var nomNationalities = db.Nationalities.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomNationalities.ToList());
        }

        // GET: NomNationalities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomNationality nomNationality = db.Nationalities.Find(id);
            if (nomNationality == null)
            {
                return HttpNotFound();
            }
            return View(nomNationality);
        }

        // GET: NomNationalities/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: NomNationalities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] NomNationality nomNationality)
        {
            NomNationality nationality = new NomNationality();
            nationality.Name = nomNationality.Name;

            if (ModelState.IsValid)
            {
                db.Nationalities.Add(nationality);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomNationality.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomNationality.RegisteredBy);
            return View(nomNationality);
        }

        // GET: NomNationalities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomNationality nomNationality = db.Nationalities.Find(id);
            if (nomNationality == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomNationality.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomNationality.RegisteredBy);
            return View(nomNationality);
        }

        // POST: NomNationalities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] NomNationality nomNationality)
        {
            NomNationality nationalityDb = db.Nationalities.Find(nomNationality.ID);
            nationalityDb.Name = nomNationality.Name;

            if (ModelState.IsValid)
            {
                db.Entry(nationalityDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomNationality.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomNationality.RegisteredBy);
            return View(nomNationality);
        }

        // GET: NomNationalities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomNationality nomNationality = db.Nationalities.Find(id);
            if (nomNationality == null)
            {
                return HttpNotFound();
            }
            return View(nomNationality);
        }

        // POST: NomNationalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NomNationality nomNationality = db.Nationalities.Find(id);
            db.Nationalities.Remove(nomNationality);
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
