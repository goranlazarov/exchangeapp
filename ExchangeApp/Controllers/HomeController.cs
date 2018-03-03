using ExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ExchangeApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            NomRegion initial = new NomRegion();
            initial.ID = -1;
            initial.Name = "Select region";
            List<Models.NomRegion> list = new List<Models.NomRegion>();
            list.Add(initial);
            list.AddRange(db.Regions);

            NomCountry initialC = new NomCountry();
            initialC.ID = -1;
            initialC.Name = "Select country";
            List<Models.NomCountry> listCountries = new List<Models.NomCountry>();
            listCountries.Add(initialC);
            listCountries.AddRange(db.Countries);

            ViewBag.RegionId = new SelectList(list, "ID", "Name");

            ViewBag.CountryId = new SelectList(listCountries, "ID", "Name");

            var FacultyInstitutions = db.Faculties.ToList().OrderBy(l=>l.Registered).Take(5);
            return View(FacultyInstitutions);
            
        }

        public ActionResult ListFaculties(string SearchKeyword, string currentFilter, int? CountryId, int? currentCountry, int? page)
        {
            ViewBag.CurrentFilter = string.IsNullOrEmpty(SearchKeyword) ? currentFilter : SearchKeyword;
            ViewBag.CurrentCountry = CountryId.HasValue ? CountryId : currentCountry;

            var programs = from faculties in db.Faculties
                         select faculties.Program;

            ViewBag.Programs = programs.ToList().Distinct();

            int x = 0;
            NomRegion initial = new NomRegion();
            initial.ID = -1;
            initial.Name = "Select region";
            List<Models.NomRegion> list = new List<Models.NomRegion>();
            list.Add(initial);
            list.AddRange(db.Regions);

            NomCountry initialC = new NomCountry();
            initialC.ID = -1;
            initialC.Name = "Select country";
            List<Models.NomCountry> listCountries = new List<Models.NomCountry>();
            listCountries.Add(initialC);
            listCountries.AddRange(db.Countries);

            ViewBag.RegionId = new SelectList(list, "ID", "Name");

            ViewBag.CountryId = new SelectList(listCountries, "ID", "Name");

            if (SearchKeyword != null)
                page = 1;
            else
                SearchKeyword = currentFilter;

            if(!CountryId.HasValue)
            {
                CountryId = currentCountry;
            }

            var facultiesFiltered = db.Faculties.ToList();
            if(!string.IsNullOrEmpty(SearchKeyword))
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.Name.Contains(SearchKeyword)).ToList();
            }
            if(CountryId.HasValue && CountryId != -1)
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.CountryId.HasValue && f.CountryId == CountryId).ToList();
            }

            SearchViewModel svm = new SearchViewModel();
            svm.SearchKeyword = SearchKeyword;
            svm.CountryId = (CountryId.HasValue ? CountryId.Value : -1);
            ViewBag.SearchViewModel = svm;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(facultiesFiltered.OrderBy(l => l.Registered).ToPagedList(pageNumber, pageSize));

        }

        [HttpGet]
        public JsonResult getCountries(int regionId)
        {
            var countries = db.Countries.Where(c => (c.RegionId.Value == regionId && regionId!=-1) || regionId==-1).Select(c => new
            {
                ID = c.ID,
                Name = c.Name
            });

            return Json(countries.ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            DisplaySuccessMessage("Successfully opened!");

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}