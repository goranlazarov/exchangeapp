using System;
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
    public class NomApplicantHighestDegreesController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomApplicantHighestDegrees
        public ActionResult Index(int? page)
        {
            var nomApplicantHighestDegrees = db.ApplicantHighestDegrees.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(nomApplicantHighestDegrees.OrderBy(l => l.Registered).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddEditHighestDegree(int highestDegreeId)
        {
            NomApplicantHighestDegree model = new NomApplicantHighestDegree();

            if (highestDegreeId > 0)
            {
                NomApplicantHighestDegree highestDegree = db.ApplicantHighestDegrees.Find(highestDegreeId);
                model.ID = highestDegree.ID;
                model.Name = highestDegree.Name;
            }

            return PartialView("AddEditHighestDegree", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateHighestDegree(NomApplicantHighestDegree model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (model.ID > 0)
                    {
                        NomApplicantHighestDegree highestDegreeDb = db.ApplicantHighestDegrees.FirstOrDefault(x => x.ID == model.ID);
                        highestDegreeDb.ID = model.ID;
                        highestDegreeDb.Name = model.Name;

                        message = "Successfully edited highest degree!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.ApplicantHighestDegrees.Any(x => x.Name.ToLower() == model.Name.ToLower()))
                        {
                            throw new Exception("Highest degree already exists!");
                        }

                        NomApplicantHighestDegree highestDegree = new NomApplicantHighestDegree();
                        highestDegree.ID = model.ID;
                        highestDegree.Name = model.Name;

                        message = "Successfully added highest degree!";

                        db.ApplicantHighestDegrees.Add(highestDegree);
                        db.SaveChanges();
                    }

                    DisplaySuccessMessage(message);
                    return Json(true);
                }

                catch (Exception ex)
                {
                    var modelErrors = new List<string>();
                    modelErrors.Add(ex.Message);

                    return Json(modelErrors);
                }
            }
            else
            {
                var errors = GetModelStateErrors(ModelState.Values);
                return Json(errors);
            }

        }

        [HttpPost]
        public ActionResult DeleteHighestDegree(int id)
        {
            NomApplicantHighestDegree highestDegreeDb = db.ApplicantHighestDegrees.Find(id);
            db.ApplicantHighestDegrees.Remove(highestDegreeDb);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted highest degree!");
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
