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
    public class NomTypeOfExchangesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomTypeOfExchanges
        public ActionResult Index()
        {
            var nomTypeOfExchanges = db.TypesOfExchange.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);
            return View(nomTypeOfExchanges.ToList());
        }

        // GET: NomTypeOfExchanges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomTypeOfExchange nomTypeOfExchange = db.TypesOfExchange.Find(id);
            if (nomTypeOfExchange == null)
            {
                return HttpNotFound();
            }
            return View(nomTypeOfExchange);
        }

        // GET: NomTypeOfExchanges/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: NomTypeOfExchanges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] NomTypeOfExchange nomTypeOfExchange)
        {
            NomTypeOfExchange typeOfExchange = new NomTypeOfExchange();
            typeOfExchange.Name = nomTypeOfExchange.Name;

            if (ModelState.IsValid)
            {
                db.TypesOfExchange.Add(typeOfExchange);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomTypeOfExchange.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomTypeOfExchange.RegisteredBy);
            return View(nomTypeOfExchange);
        }

        // GET: NomTypeOfExchanges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomTypeOfExchange nomTypeOfExchange = db.TypesOfExchange.Find(id);
            if (nomTypeOfExchange == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomTypeOfExchange.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomTypeOfExchange.RegisteredBy);
            return View(nomTypeOfExchange);
        }

        // POST: NomTypeOfExchanges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] NomTypeOfExchange nomTypeOfExchange)
        {
            NomTypeOfExchange typeOfExchangeDb = db.TypesOfExchange.Find(nomTypeOfExchange.ID);
            typeOfExchangeDb.Name = nomTypeOfExchange.Name;

            if (ModelState.IsValid)
            {
                db.Entry(typeOfExchangeDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", nomTypeOfExchange.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", nomTypeOfExchange.RegisteredBy);
            return View(nomTypeOfExchange);
        }

        // GET: NomTypeOfExchanges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NomTypeOfExchange nomTypeOfExchange = db.TypesOfExchange.Find(id);
            if (nomTypeOfExchange == null)
            {
                return HttpNotFound();
            }
            return View(nomTypeOfExchange);
        }

        // POST: NomTypeOfExchanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NomTypeOfExchange nomTypeOfExchange = db.TypesOfExchange.Find(id);
            db.TypesOfExchange.Remove(nomTypeOfExchange);
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
