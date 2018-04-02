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
        public ActionResult Index(int? flag)
        {
            if (flag.HasValue)
                ViewBag.LinkToOther = (flag.Value == 1 ? "Student" : "Faculty");
            else
                ViewBag.LinkToOther = "Faculty";


            AddFields();

            var ReturnModel = new FacultiesViewModel();
            var facultiesFiltered = db.Faculties.ToList();

            if (flag.HasValue)
            {
                if (!String.IsNullOrEmpty(ViewBag.LinkToOther.ToString()) && ViewBag.LinkToOther.ToString().Equals("Student"))
                {
                    var facultiesTeachersFiltered = facultiesFiltered.Where(f => (f.IsFeatured.HasValue && f.IsFeatured.Value) &&
                                                        f.FacultyPlacesAvailable.HasValue && f.FacultyPlacesAvailable.Value > 0 &&
                                                            f.FacultyApplicationDate.HasValue && f.FacultyEnrollmentDate.HasValue).ToList().OrderBy(l => l.Registered).Take(10);
                    if (facultiesTeachersFiltered.ToList().Count < 10)
                    {
                        var facultiesTeachersFilteredNotFeatured = facultiesFiltered.Where(f =>
                                                (!f.IsFeatured.HasValue || (f.IsFeatured.HasValue && !f.IsFeatured.Value)) &&
                                                f.FacultyPlacesAvailable.HasValue && f.FacultyPlacesAvailable.Value > 0 &&
                                                f.FacultyApplicationDate.HasValue && f.FacultyEnrollmentDate.HasValue).ToList().OrderBy(l => l.Registered).Take(10 - facultiesTeachersFiltered.ToList().Count);


                        facultiesTeachersFiltered = facultiesTeachersFiltered.Concat(facultiesTeachersFilteredNotFeatured);
                    }
                    ReturnModel.TeacherPlaces = new List<Faculty>(facultiesTeachersFiltered);
                    return View(ReturnModel);
                }

            }

            var facultiesFilteredFinal = facultiesFiltered.Where(f => (f.IsFeatured.HasValue && f.IsFeatured.Value) &&
                                               f.StudentPlacesAvailable.HasValue && f.StudentPlacesAvailable.Value > 0 &&
                                               f.StudentApplicationDate.HasValue && f.StudentEnrollmentDate.HasValue).ToList().OrderBy(l => l.Registered).Take(10);
            if (facultiesFilteredFinal.ToList().Count < 10)
            {
                var facultiesStudentsFilteredNotFeatured = facultiesFiltered.Where(f =>
                                        (!f.IsFeatured.HasValue || (f.IsFeatured.HasValue && !f.IsFeatured.Value)) &&
                                        f.StudentPlacesAvailable.HasValue && f.StudentPlacesAvailable.Value > 0 &&
                                        f.StudentApplicationDate.HasValue && f.StudentEnrollmentDate.HasValue).ToList().OrderBy(l => l.Registered).Take(10 - facultiesFilteredFinal.ToList().Count);

                facultiesFilteredFinal = facultiesFilteredFinal.Concat(facultiesStudentsFilteredNotFeatured);
            }
            ReturnModel.StudentsPlaces = new List<Faculty>(facultiesFilteredFinal);

            return View(ReturnModel);

        }

        public ActionResult ListFaculties(bool? StudentSelected, bool? FacultySelected, bool? currStud, bool? currFac, string SearchProgram, string currentProgram, string SearchKeyword, string currentFilter, int? CountryId, int? currentCountry, int? page)
        {
            ViewBag.CurrentFilter = string.IsNullOrEmpty(SearchKeyword) ? currentFilter : SearchKeyword;
            ViewBag.CurrentCountry = CountryId.HasValue ? CountryId : currentCountry;
            ViewBag.CurrentProgram = string.IsNullOrEmpty(SearchProgram) ? currentProgram : SearchProgram;
            ViewBag.CurrentStudent = StudentSelected.HasValue ? StudentSelected : currStud;
            ViewBag.CurrentFaculty = FacultySelected.HasValue ? FacultySelected : currFac;

            var programs = from faculties in db.Faculties
                           select faculties.Program;

            ViewBag.Programs = programs.ToList().Distinct().Take(10);

            AddFields();

            if (SearchKeyword != null)
                page = 1;
            else
                SearchKeyword = currentFilter;

            if (!CountryId.HasValue)
            {
                CountryId = currentCountry;
            }

            if (string.IsNullOrEmpty(SearchProgram))
            {
                SearchProgram = currentProgram;
            }

            if (!StudentSelected.HasValue)
            {
                StudentSelected = currStud;
            }

            if (!FacultySelected.HasValue)
            {
                FacultySelected = currFac;
            }

            var facultiesFiltered = db.Faculties.ToList();
            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.Name.Contains(SearchKeyword)).ToList();
            }
            if (CountryId.HasValue && CountryId != -1)
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.CountryId.HasValue && f.CountryId == CountryId).ToList();
            }
            if (!string.IsNullOrEmpty(SearchProgram))
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.Program.Contains(SearchProgram)).ToList();
            }

            //TO DO - logika so datumi za sporedba so momentalen datum
            if ((StudentSelected.HasValue && StudentSelected.Value) || (FacultySelected.HasValue && FacultySelected.Value))
            {
                facultiesFiltered = facultiesFiltered.Where(f =>
                                            (f.StudentPlacesAvailable.HasValue && f.StudentPlacesAvailable.Value > 0 &&
                                                f.StudentApplicationDate.HasValue && f.StudentEnrollmentDate.HasValue &&
                                                (StudentSelected.HasValue && StudentSelected.Value)
                                        ) ||
                                           (f.FacultyPlacesAvailable.HasValue && f.FacultyPlacesAvailable.Value > 0 &&
                                                f.FacultyApplicationDate.HasValue && f.FacultyEnrollmentDate.HasValue &&
                                                (FacultySelected.HasValue && FacultySelected.Value)
                                        )
                                        ).ToList();
            }

            SearchViewModel svm = new SearchViewModel();
            svm.SearchKeyword = SearchKeyword;
            svm.CountryId = (CountryId.HasValue ? CountryId.Value : -1);
            ViewBag.SearchViewModel = svm;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(facultiesFiltered.OrderByDescending(l => l.IsFeatured).ThenByDescending(l => l.Registered).ToPagedList(pageNumber, pageSize));

        }

        public void AddFields()
        {
            NomRegion initial = new NomRegion();
            initial.ID = -1;
            initial.Name = "Select region";
            List<Models.NomRegion> list = new List<Models.NomRegion>();
            list.Add(initial);
            list.AddRange(db.Regions);

            NomCountry initialC = new NomCountry();
            initialC.ID = -1;
            initialC.Name = "Select country/state";
            List<Models.NomCountry> listCountries = new List<Models.NomCountry>();
            listCountries.Add(initialC);
            listCountries.AddRange(db.Countries);

            ViewBag.RegionId = new SelectList(list, "ID", "Name");

            ViewBag.CountryId = new SelectList(listCountries, "ID", "Name");
        }

        [HttpGet]
        public JsonResult getCountries(int regionId)
        {
            var countries = db.Countries.Where(c => (c.RegionId.Value == regionId && regionId != -1) || regionId == -1).Select(c => new
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

        public ActionResult Error()
        {
            return View("~/Views/Base/ErrorPage.cshtml");
        }
    }
}