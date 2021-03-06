﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExchangeApp.Models;
using PagedList;

namespace ExchangeApp.Controllers
{
    public class FacultiesController : BaseAdminController
    {

        // GET: Faculties
        public ActionResult Index(int? page,int ? RegionId, int? currentRegion)
        {
            ViewBag.CurrentRegion = RegionId.HasValue ? RegionId : currentRegion;

            NomRegion initial = new NomRegion();
            initial.ID = -1;
            initial.Name = "All regions";
            List<Models.NomRegion> list = new List<Models.NomRegion>();
            list.Add(initial);
            list.AddRange(db.Regions);
            
            ViewBag.RegionId = new SelectList(list, "ID", "Name", ViewBag.CurrentRegion);

            if (!RegionId.HasValue)
            {
                RegionId = currentRegion;
            }

            var faculties = db.Faculties.Include(f => f.CountryObj).Include(f => f.FacultyTypeOfExchangeObj).Include(f => f.LastUpdatedByUser).Include(f => f.RegisteredByUser).Include(f => f.StudentTypeOfExchangeObj);

            if (RegionId.HasValue && RegionId != -1)
            {
                faculties = faculties.Where(f => f.CountryObj != null && f.CountryObj.RegionId == RegionId);
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(faculties.OrderBy(l => l.Name).ToPagedList(pageNumber, pageSize));
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
            List<NomCountry> countriesList = db.Countries.OrderBy(x => x.Name).ToList();
            ViewBag.CountriesList = new SelectList(countriesList, "ID", "Name");

            List<NomTypeOfExchange> typesOfExchangesStudent = db.TypesOfExchange.Where(t => t.Student.HasValue && t.Student.Value).ToList();
            List<NomTypeOfExchange> typesOfExchangesFaculty = db.TypesOfExchange.Where(t => t.Faculty.HasValue && t.Faculty.Value).ToList();
            ViewBag.TypesOfExchangeStudent = new SelectList(typesOfExchangesStudent, "ID", "Name");
            ViewBag.TypesOfExchangeFaculty = new SelectList(typesOfExchangesFaculty, "ID", "Name");

            List<Subject> subjects = db.Subjects.ToList();
            ViewBag.Subjects = new MultiSelectList(subjects, "ID", "Name");

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName");

            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Program,Email,Description,AgreementNumber,Website,CountryId,DateOfMatriculation,AccreditationNumber,DateOfAccreditation,StudentPlacesAvailable,StudentApplicationDate,StudentEnrollmentDate,FacultyPlacesAvailable,FacultyApplicationDate,FacultyEnrollmentDate,StudentTypeOfExchangeId,FacultyTypeOfExchangeId, IsFeatured, Display, SelectedSubjectsIds")] Faculty faculty, bool StudentSelected, bool FacultySelected, HttpPostedFileBase File)
        {

            if (db.Faculties.Any(x => x.Name.ToLower() == faculty.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "College/university with that name already exists");
            }

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
                if (faculty.SelectedSubjectsIds == null || faculty.SelectedSubjectsIds.Count() == 0)
                {
                    ModelState.AddModelError("SelectedSubjectsIds", "Please choose at least one course");
                }
            }

            if (ModelState.IsValid)
            {
                if (File != null)
                {
                    if (IsValidImage(File))
                    {
                        byte[] image = new byte[File.ContentLength];
                        File.InputStream.Read(image, 0, image.Length);
                        faculty.LogoImage = image;
                    }
                    else
                    {
                        DisplayErrorMessage("Image format is not valid or image is bigger than 3MB!");
                        return RedirectToAction("Index");
                    }
                }

                if (faculty.SelectedSubjectsIds != null)
                {
                    for (var i = 0; i < faculty.SelectedSubjectsIds.Count(); i++)
                    {
                        FacultyCourses fc = new FacultyCourses();
                        fc.FacultyId = faculty.ID;
                        fc.FacultyObj = faculty;

                        var SubjectObj = db.Subjects.Find(faculty.SelectedSubjectsIds[i]);
                        fc.SubjectId = faculty.SelectedSubjectsIds[i];
                        fc.SubjectObj = SubjectObj;

                        db.FacultyCourses.Add(fc);
                    }
                }

                db.Faculties.Add(faculty);
                db.SaveChanges();
                DisplaySuccessMessage("Successfully added new faculty!");
                return RedirectToAction("Index");
            }

            List<NomCountry> countriesList = db.Countries.OrderBy(x => x.Name).ToList();
            ViewBag.CountriesList = new SelectList(countriesList, "ID", "Name");

            List<NomTypeOfExchange> typesOfExchangesStudent = db.TypesOfExchange.Where(t => t.Student.HasValue && t.Student.Value).ToList();
            List<NomTypeOfExchange> typesOfExchangesFaculty = db.TypesOfExchange.Where(t => t.Faculty.HasValue && t.Faculty.Value).ToList();
            ViewBag.TypesOfExchangeStudent = new SelectList(typesOfExchangesStudent, "ID", "Name");
            ViewBag.TypesOfExchangeFaculty = new SelectList(typesOfExchangesFaculty, "ID", "Name");

            List<Subject> subjects = db.Subjects.ToList();
            ViewBag.Subjects = new MultiSelectList(subjects, "ID", "Name");

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", faculty.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", faculty.RegisteredBy);

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

            List<NomTypeOfExchange> typesOfExchangesStudent = db.TypesOfExchange.Where(t=>t.Student.HasValue && t.Student.Value).ToList();
            List<NomTypeOfExchange> typesOfExchangesFaculty = db.TypesOfExchange.Where(t => t.Faculty.HasValue && t.Faculty.Value).ToList();
            List<Subject> subjects = db.Subjects.ToList();
            ViewBag.Subjects = new MultiSelectList(subjects, "ID", "Name");

     
            ViewBag.FacultyTypeOfExchangeId = new SelectList(typesOfExchangesFaculty, "ID", "Name", faculty.FacultyTypeOfExchangeId);
            ViewBag.StudentTypeOfExchangeId = new SelectList(typesOfExchangesStudent, "ID", "Name", faculty.StudentTypeOfExchangeId);

            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "ID", "Name", faculty.CountryId);
            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", faculty.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", faculty.RegisteredBy);


            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Program,Email,Website,CountryId,DateOfMatriculation,AccreditationNumber,DateOfAccreditation,AgreementNumber, Description,StudentPlacesAvailable,StudentApplicationDate,StudentEnrollmentDate,FacultyPlacesAvailable,FacultyApplicationDate,FacultyEnrollmentDate,StudentTypeOfExchangeId,FacultyTypeOfExchangeId,Registered,RegisteredBy,LastUpdated,LastUpdatedBy,RowVersion, IsFeatured, Display, LogoImage,SelectedSubjectsIds")] Faculty faculty, bool StudentSelected, bool FacultySelected, HttpPostedFileBase File)
        {

            if (db.Faculties.Any(x => x.Name.ToLower() == faculty.Name.ToLower() && x.ID != faculty.ID))
            {
                ModelState.AddModelError("Name", "College/university with that name already exists");
            }

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
                    ModelState.AddModelError("FacultyEnrollmentDate", "Please choose engagement date");
                }
                if (faculty.SelectedSubjectsIds == null || faculty.SelectedSubjectsIds.Count() == 0)
                {
                    ModelState.AddModelError("SelectedSubjectsIds", "Please choose at least one course");
                }
            }

            if (ModelState.IsValid)
            {
                if (File != null)
                {
                    if (IsValidImage(File))
                    {
                        byte[] image = new byte[File.ContentLength];
                        File.InputStream.Read(image, 0, image.Length);
                        faculty.LogoImage = image;
                    }
                    else
                    {
                        DisplayErrorMessage("Image format is not valid or image is bigger than 3MB!");
                        return RedirectToAction("Index");
                    }
                }

                if (faculty.SelectedSubjectsIds != null)
                {
                    db.FacultyCourses.RemoveRange(db.FacultyCourses.Where(x => x.FacultyId == faculty.ID));

                    for (var i = 0; i < faculty.SelectedSubjectsIds.Count(); i++)
                    {
                        FacultyCourses fc = new FacultyCourses();
                        fc.FacultyId = faculty.ID;
                        fc.FacultyObj = faculty;

                        var SubjectObj = db.Subjects.Find(faculty.SelectedSubjectsIds[i]);
                        fc.SubjectId = faculty.SelectedSubjectsIds[i];
                        fc.SubjectObj = SubjectObj;

                        db.FacultyCourses.Add(fc);
                    }
                }
                else
                {
                    db.FacultyCourses.RemoveRange(db.FacultyCourses.Where(x => x.FacultyId == faculty.ID));
                }

                db.Entry(faculty).State = EntityState.Modified;
                db.SaveChanges();
                DisplaySuccessMessage("Successfully edited faculty!");
                return RedirectToAction("Index");
            }

            List<NomCountry> countriesList = db.Countries.ToList();
            ViewBag.CountriesList = new SelectList(countriesList, "ID", "Name");

            List<NomTypeOfExchange> typesOfExchangesStudent = db.TypesOfExchange.Where(t => t.Student.HasValue && t.Student.Value).ToList();
            List<NomTypeOfExchange> typesOfExchangesFaculty = db.TypesOfExchange.Where(t => t.Faculty.HasValue && t.Faculty.Value).ToList();
            ViewBag.TypesOfExchangeStudent = new SelectList(typesOfExchangesStudent, "ID", "Name");
            ViewBag.TypesOfExchangeFaculty = new SelectList(typesOfExchangesFaculty, "ID", "Name");

            List<Subject> subjects = db.Subjects.ToList();
            ViewBag.Subjects = new MultiSelectList(subjects, "ID", "Name");

            ViewBag.LastUpdatedBy = new SelectList(db.Users, "Id", "FirstName", faculty.LastUpdatedBy);
            ViewBag.RegisteredBy = new SelectList(db.Users, "Id", "FirstName", faculty.RegisteredBy);

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

        public bool IsValidImage(HttpPostedFileBase postedFile)
        {
            var isValid = true;

            if (postedFile.ContentType.ToLower() != "image/jpg" &&
             postedFile.ContentType.ToLower() != "image/jpeg" &&
             postedFile.ContentType.ToLower() != "image/png")
            {
                isValid = false;
            }

            if (postedFile.ContentLength > 3145728)
            {
                isValid = false;
            }

            return isValid;

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DeleteImage(int id)
        {
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    faculty.LogoImage = null;
                    db.Entry(faculty).State = EntityState.Modified;
                    db.SaveChanges();
                    DisplaySuccessMessage("The faculty image was successfully deleted!");
                    return Json(true);
                }
            }

            return View(faculty);

        }

        public JsonResult CheckAgreementNumber(string number)
        {
            if (db.Faculties.Any(x => x.AgreementNumber.Equals(number)))
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public JsonResult GetCourses(int id)
        {
            var faculty = db.Faculties.Find(id);
            List<int> courses = new List<int>();

            foreach (var crs in faculty.Courses)
            {
                courses.Add(crs.SubjectObj.ID);
            }

            return this.Json(courses, JsonRequestBehavior.AllowGet);
        }

    }
}
