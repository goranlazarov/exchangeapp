using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExchangeApp.Models;
using PagedList;

namespace ExchangeApp.Controllers
{
    public class ManualController : BaseAdminController
    {
        public ActionResult Index()
        {
            try
            {
                string pathFile = @"~/App_Data/Manual.txt";
                var fileContents = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(pathFile));
                ManualViewModel mvm = new ManualViewModel();
                mvm.ManualText = fileContents;
                return View(mvm);
            }
            catch(Exception e)
            {
                DisplayErrorMessage("Error ocurred, failed loading current manual text, enter new and save!");
                return View();
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Save(ManualViewModel model)
        {
            try
            {
                string pathFile = @"~/App_Data/Manual.txt";
                var fileContents = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(pathFile));
                System.IO.File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath(pathFile), model.ManualText);
                DisplaySuccessMessage("Successfully updated manual text!");
                return RedirectToAction("Index", "Admin");
                
            }
            catch (Exception e)
            {
                DisplayErrorMessage("Error ocurred, failed updating manual text!");
                return RedirectToAction("Index");
            }
            
        }
    }
}
