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
    public class NomCountriesController : BaseAdminController
    {

        // GET: NomCountries
        public ActionResult Index(int? page)
        {
            var nomCountries = db.Countries.Include(n => n.LastUpdatedByUser).Include(n => n.RegionObj).Include(n => n.RegisteredByUser);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(nomCountries.OrderBy(l => l.Registered).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddEditCountry(int countryId)
        {
            List<NomRegion> regions = db.Regions.ToList();
            ViewBag.RegionsList = new SelectList(regions, "ID", "Name");

            NomCountry model = new NomCountry();

            if (countryId > 0)
            {
                NomCountry country = db.Countries.Find(countryId);
                model.ID = country.ID;
                model.Name = country.Name;
                model.RegionId = country.RegionId;
                model.RegionObj = country.RegionObj;
            }

            return PartialView("AddEditCountry", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateCountry(NomCountry model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (model.ID > 0)
                    {
                        NomCountry countryDb = db.Countries.FirstOrDefault(x => x.ID == model.ID);
                        countryDb.ID = model.ID;
                        countryDb.Name = model.Name;
                        countryDb.RegionObj = model.RegionObj;
                        countryDb.RegionId = model.RegionId;

                        message = "Successfully edited country!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.Countries.Any(x => x.Name.ToLower() == model.Name.ToLower() && x.RegionId == model.RegionId))
                        {
                            throw new Exception("Country already exists!");
                        }

                        NomCountry country = new NomCountry();
                        country.ID = model.ID;
                        country.Name = model.Name;

                        NomRegion region = db.Regions.Find(model.RegionId);
                        country.RegionObj = region;
                        country.RegionId = region.ID;

                        message = "Successfully added country!";

                        db.Countries.Add(country);
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
        public ActionResult DeleteCountry(int id)
        {
            NomCountry country = db.Countries.Find(id);
            db.Countries.Remove(country);
            db.SaveChanges();

            DisplaySuccessMessage("Successfully deleted country!");
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
