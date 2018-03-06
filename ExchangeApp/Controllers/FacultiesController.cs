﻿using System;
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
    public class FacultiesController : BaseAdminController
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
            List<NomCountry> countriesList = db.Countries.ToList();
            ViewBag.CountriesList = new SelectList(countriesList, "ID", "Name");

            List<NomTypeOfExchange> typesOfExchanges = db.TypesOfExchange.ToList();
            ViewBag.TypesOfExchange = new SelectList(typesOfExchanges, "ID", "Name");

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Program,Email,Description,AgreementNumber,Website,CountryId,DateOfMatriculation,AccreditationNumber,DateOfAccreditation,StudentPlacesAvailable,StudentApplicationDate,StudentEnrollmentDate,FacultyPlacesAvailable,FacultyApplicationDate,FacultyEnrollmentDate,StudentTypeOfExchangeId,FacultyTypeOfExchangeId")] Faculty faculty, bool StudentSelected, bool FacultySelected)
        {
            if (!StudentSelected && !FacultySelected)
            {
                ModelState.AddModelError("", "Please select type of exchange that you offer");
            }

            if (StudentSelected)
            {
                if (faculty.StudentTypeOfExchangeId == null)
                {
                    ModelState.AddModelError("StudentTypeOfExchangeId", "Please choose student type of exchange");
                }
                if (faculty.StudentPlacesAvailable == null || faculty.StudentPlacesAvailable == 0)
                {
                    ModelState.AddModelError("StudentPlacesAvailable", "Please enter student places available");
                }
                if (faculty.StudentApplicationDate == null)
                {
                    ModelState.AddModelError("StudentApplicationDate", "Please choose application date");
                }
                if (faculty.StudentEnrollmentDate == null)
                {
                    ModelState.AddModelError("StudentEnrollmentDate", "Please choose enrollment date");
                }
            }

            if (FacultySelected)
            {
                if (faculty.FacultyTypeOfExchangeId == null)
                {
                    ModelState.AddModelError("FacultyTypeOfExchangeId", "Please choose faculty type of exchange");
                }
                if (faculty.FacultyPlacesAvailable == null || faculty.FacultyPlacesAvailable == 0)
                {
                    ModelState.AddModelError("FacultyPlacesAvailable", "Please enter faculty places available");
                }
                if (faculty.FacultyApplicationDate == null)
                {
                    ModelState.AddModelError("FacultyApplicationDate", "Please choose application date");
                }
                if (faculty.FacultyEnrollmentDate == null)
                {
                    ModelState.AddModelError("FacultyEnrollmentDate", "Please choose enrollment date");
                }
            }

            if (ModelState.IsValid)
            {
                db.Faculties.Add(faculty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<NomCountry> countriesList = db.Countries.ToList();
            ViewBag.CountriesList = new SelectList(countriesList, "ID", "Name");

            List<NomTypeOfExchange> typesOfExchanges = db.TypesOfExchange.ToList();
            ViewBag.TypesOfExchange = new SelectList(typesOfExchanges, "ID", "Name");

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", faculty.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", faculty.RegisteredBy);

            DisplaySuccessMessage("Successfully added new faculty!");
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
        public ActionResult Edit([Bind(Include = "ID,Name,Program,Email,Website,CountryId,DateOfMatriculation,AccreditationNumber,DateOfAccreditation,AgreementNumber, Description,StudentPlacesAvailable,StudentApplicationDate,StudentEnrollmentDate,FacultyPlacesAvailable,FacultyApplicationDate,FacultyEnrollmentDate,StudentTypeOfExchangeId,FacultyTypeOfExchangeId,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion")] Faculty faculty, bool StudentSelected, bool FacultySelected)
        {
            if (!StudentSelected && !FacultySelected)
            {
                ModelState.AddModelError("", "Please select type of exchange that you offer");
            }

            if (StudentSelected)
            {
                if (faculty.StudentTypeOfExchangeId == null)
                {
                    ModelState.AddModelError("StudentTypeOfExchangeId", "Please choose student type of exchange");
                }
                if (faculty.StudentPlacesAvailable == null || faculty.StudentPlacesAvailable == 0)
                {
                    ModelState.AddModelError("StudentPlacesAvailable", "Please enter student places available");
                }
                if (faculty.StudentApplicationDate == null)
                {
                    ModelState.AddModelError("StudentApplicationDate", "Please choose application date");
                }
                if (faculty.StudentEnrollmentDate == null)
                {
                    ModelState.AddModelError("StudentEnrollmentDate", "Please choose enrollment date");
                }
            }

            if (FacultySelected)
            {
                if (faculty.FacultyTypeOfExchangeId == null)
                {
                    ModelState.AddModelError("FacultyTypeOfExchangeId", "Please choose faculty type of exchange");
                }
                if (faculty.FacultyPlacesAvailable == null || faculty.FacultyPlacesAvailable == 0)
                {
                    ModelState.AddModelError("FacultyPlacesAvailable", "Please enter faculty places available");
                }
                if (faculty.FacultyApplicationDate == null)
                {
                    ModelState.AddModelError("FacultyApplicationDate", "Please choose application date");
                }
                if (faculty.FacultyEnrollmentDate == null)
                {
                    ModelState.AddModelError("FacultyEnrollmentDate", "Please choose enrollment date");
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(faculty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<NomCountry> countriesList = db.Countries.ToList();
            ViewBag.CountriesList = new SelectList(countriesList, "ID", "Name");

            List<NomTypeOfExchange> typesOfExchanges = db.TypesOfExchange.ToList();
            ViewBag.TypesOfExchange = new SelectList(typesOfExchanges, "ID", "Name");

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", faculty.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", faculty.RegisteredBy);

            DisplaySuccessMessage("Successfully edited faculty!");
            return View(faculty);
        }

        [HttpPost]
        public ActionResult DeleteFaculty(int id)
        {
            Faculty faculty = db.Faculties.Find(id);
            db.Faculties.Remove(faculty);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted faculty!");
            return Json(true);
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
