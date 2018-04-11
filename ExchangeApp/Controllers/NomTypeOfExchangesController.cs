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
    public class NomTypeOfExchangesController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomTypeOfExchanges
        public ActionResult Index(int? page)
        {
            var nomTypeOfExchanges = db.TypesOfExchange.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(nomTypeOfExchanges.OrderBy(l => l.Registered).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddEditTypeOfExchange(int typeId)
        {
            NomTypeOfExchange model = new NomTypeOfExchange();

            if (typeId > 0)
            {
                NomTypeOfExchange typeOfExchange = db.TypesOfExchange.Find(typeId);
                model.ID = typeOfExchange.ID;
                model.Name = typeOfExchange.Name;
                model.Faculty = typeOfExchange.Faculty;
                model.Student = typeOfExchange.Student;

            }

            return PartialView("AddEditTypeOfExchange", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateTypeOfExchange(NomTypeOfExchange model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (model.ID > 0)
                    {
                        NomTypeOfExchange typeDb = db.TypesOfExchange.FirstOrDefault(x => x.ID == model.ID);
                        typeDb.ID = model.ID;
                        typeDb.Name = model.Name;
                        typeDb.Student = model.Student;
                        typeDb.Faculty = model.Faculty;

                        message = "Successfully edited type of exchange!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.TypesOfExchange.Any(x => x.Name.ToLower() == model.Name.ToLower()))
                        {
                            throw new Exception("Type of exchange already exists!");
                        }

                        NomTypeOfExchange type = new NomTypeOfExchange();
                        type.ID = model.ID;
                        type.Name = model.Name;
                        type.Faculty = model.Faculty;
                        type.Student = model.Student;

                        message = "Successfully added type of exchange!";

                        db.TypesOfExchange.Add(type);
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
        public ActionResult DeleteTypeOfExchange(int id)
        {
            NomTypeOfExchange type = db.TypesOfExchange.Find(id);
            db.TypesOfExchange.Remove(type);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted type of exchange!");
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
