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
    public class NomRegionsController : BaseAdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NomRegions
        public ActionResult Index(int? page)
        {
            var nomRegions = db.Regions.Include(n => n.LastUpdatedByUser).Include(n => n.RegisteredByUser);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(nomRegions.OrderBy(l => l.Registered).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddEditRegion(int regionId)
        {
            NomRegion model = new NomRegion();

            if (regionId > 0)
            {
                NomRegion region = db.Regions.Find(regionId);
                model.ID = region.ID;
                model.Name = region.Name;
            }

            return PartialView("AddEditRegion", model);

        }

        [HttpPost]
        public ActionResult CreateUpdateRegion(NomRegion model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = "";

                    if (model.ID > 0)
                    {
                        NomRegion regionDb = db.Regions.FirstOrDefault(x => x.ID == model.ID);
                        regionDb.ID = model.ID;
                        regionDb.Name = model.Name;

                        message = "Successfully edited region!";

                        db.SaveChanges();
                    }
                    else
                    {
                        if (db.Regions.Any(x => x.Name.ToLower() == model.Name.ToLower()))
                        {
                            throw new Exception("Region with that name already exists!");
                        }

                        NomRegion region = new NomRegion();
                        region.ID = model.ID;
                        region.Name = model.Name;

                        message = "Successfully added region!";

                        db.Regions.Add(region);
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
        public ActionResult DeleteRegion(int id)
        {
            try
            {
                NomRegion regionDb = db.Regions.Find(id);
                db.Regions.Remove(regionDb);
                db.SaveChanges();

                DisplaySuccessMessage("Successfully deleted region!");
                return Json(true);
            }
            catch (Exception ex){
                DisplayErrorMessage("Region can't be deleted because there are countries/states in it!");
                return Json(false);
            }
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
