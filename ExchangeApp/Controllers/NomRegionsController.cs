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
    public class NomRegionsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomRegions
        public ActionResult Index()
        {
            var nomRegions = db.Regions.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomRegions.ToList());
        }

        // GET: NomRegions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomRegion nomRegion = db.Regions.Find(id);
            if (nomRegion == null)
            {
                return HttpNotFound();
            }
            return View(nomRegion);
        }

        // GET: NomRegions/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: NomRegions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] NomRegion nomRegion)
        {
            NomRegion region = new NomRegion();
            region.Name = nomRegion.Name;

            if (ModelState.IsValid)
            {
                db.Regions.Add(region);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomRegion.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomRegion.RegisteredBy);
            return View(nomRegion);
        }

        // GET: NomRegions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomRegion nomRegion = db.Regions.Find(id);
            if (nomRegion == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomRegion.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomRegion.RegisteredBy);
            return View(nomRegion);
        }

        // POST: NomRegions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] NomRegion nomRegion)
        {
            NomRegion regionDb = db.Regions.Find(nomRegion.ID);
            regionDb.Name = nomRegion.Name;

            if (ModelState.IsValid)
            {
                db.Entry(regionDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomRegion.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomRegion.RegisteredBy);
            return View(nomRegion);
        }

        // GET: NomRegions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomRegion nomRegion = db.Regions.Find(id);
            if (nomRegion == null)
            {
                return HttpNotFound();
            }
            return View(nomRegion);
        }

        // POST: NomRegions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NomRegion nomRegion = db.Regions.Find(id);
            db.Regions.Remove(nomRegion);
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
