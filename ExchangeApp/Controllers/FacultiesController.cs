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
    public class FacultiesController : BaseController
    {
        
        // GET: Faculties
        public ActionResult Index()
        {
            var faculties = db.Faculties.Include(f => f.CountryObj).Include(f => f.FacultyTypeOfExchangeObj).Include(f => f.LastUpdatedByUser).Include(f => f.RegisteredByUser).Include(f => f.StudentTypeOfExchangeObj);
            return View(faculties.ToList());
        }

        // GET: Faculties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // GET: Faculties/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "ID", "Name");
            ViewBag.FacultyTypeOfExchangeId = new SelectList(db.TypesOfExchange, "ID", "Name");
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.StudentTypeOfExchangeId = new SelectList(db.TypesOfExchange, "ID", "Name");
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Program,Email,Website,CountryId,DateOfMatriculation,AccreditationNumber,DateOfAccreditation,StudentPlacesAvailable,StudentApplicationDate,StudentEnrollmentDate,FacultyPlacesAvailable,FacultyApplicationDate,FacultyEnrollmentDate,StudentTypeOfExchangeId,FacultyTypeOfExchangeId,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                db.Faculties.Add(faculty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "ID", "Name", faculty.CountryId);
            ViewBag.FacultyTypeOfExchangeId = new SelectList(db.TypesOfExchange, "ID", "Name", faculty.FacultyTypeOfExchangeId);
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", faculty.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", faculty.RegisteredBy);
            ViewBag.StudentTypeOfExchangeId = new SelectList(db.TypesOfExchange, "ID", "Name", faculty.StudentTypeOfExchangeId);
            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "ID", "Name", faculty.CountryId);
            ViewBag.FacultyTypeOfExchangeId = new SelectList(db.TypesOfExchange, "ID", "Name", faculty.FacultyTypeOfExchangeId);
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", faculty.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", faculty.RegisteredBy);
            ViewBag.StudentTypeOfExchangeId = new SelectList(db.TypesOfExchange, "ID", "Name", faculty.StudentTypeOfExchangeId);

            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Program,Email,Website,CountryId,DateOfMatriculation,AccreditationNumber,DateOfAccreditation,StudentPlacesAvailable,StudentApplicationDate,StudentEnrollmentDate,FacultyPlacesAvailable,FacultyApplicationDate,FacultyEnrollmentDate,StudentTypeOfExchangeId,FacultyTypeOfExchangeId,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(faculty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "ID", "Name", faculty.CountryId);
            ViewBag.FacultyTypeOfExchangeId = new SelectList(db.TypesOfExchange, "ID", "Name", faculty.FacultyTypeOfExchangeId);
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", faculty.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", faculty.RegisteredBy);
            ViewBag.StudentTypeOfExchangeId = new SelectList(db.TypesOfExchange, "ID", "Name", faculty.StudentTypeOfExchangeId);
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Faculty faculty = db.Faculties.Find(id);
            db.Faculties.Remove(faculty);
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
