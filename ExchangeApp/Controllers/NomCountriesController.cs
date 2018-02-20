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
    public class NomCountriesController : BaseController
    {
        
        // GET: NomCountries
        public ActionResult Index()
        {
            var nomCountries = db.Countries.Include(n => n.LastUpdatedByUser).Include(n => n.RegionObj).Include(n => n.RegisteredByUser);
            return View(nomCountries.ToList());
        }

        // GET: NomCountries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomCountry nomCountry = db.Countries.Find(id);
            if (nomCountry == null)
            {
                return HttpNotFound();
            }
            return View(nomCountry);
        }

        // GET: NomCountries/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegionId = new SelectList(db.Regions, "ID", "Name");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: NomCountries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,RegionId")] NomCountry nomCountry)
        {
            NomCountry countryToInsert = new NomCountry();
            countryToInsert.Name = nomCountry.Name;
            countryToInsert.RegionId = nomCountry.RegionId;

            if (ModelState.IsValid)
            {
                db.Countries.Add(countryToInsert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomCountry.LastUpdatedBy);
            ViewBag.RegionId = new SelectList(db.Regions, "ID", "Name", nomCountry.RegionId);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomCountry.RegisteredBy);
            return View(nomCountry);
        }

        // GET: NomCountries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomCountry nomCountry = db.Countries.Find(id);
            if (nomCountry == null)
            {
                return HttpNotFound();
            }
          
            ViewBag.RegionId = new SelectList(db.Regions, "ID", "Name", nomCountry.RegionId);
            return View(nomCountry);
        }

        // POST: NomCountries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,RegionId,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] NomCountry nomCountry)
        {
            NomCountry country = db.Countries.Find(nomCountry.ID);

            country.Name = nomCountry.Name;
            country.RegionId = nomCountry.RegionId;
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomCountry.LastUpdatedBy);
            ViewBag.RegionId = new SelectList(db.Regions, "ID", "Name", nomCountry.RegionId);
            //ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomCountry.RegisteredBy);
            return View(nomCountry);
        }

        // GET: NomCountries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomCountry nomCountry = db.Countries.Find(id);
            if (nomCountry == null)
            {
                return HttpNotFound();
            }
            return View(nomCountry);
        }

        // POST: NomCountries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NomCountry nomCountry = db.Countries.Find(id);
            db.Countries.Remove(nomCountry);
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
