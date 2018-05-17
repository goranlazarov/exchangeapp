using ExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Core.Objects;

namespace ExchangeApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(int? flag)
        {
            string pathFile = @"~/App_Data/Manual.txt";
            var fileContents = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(pathFile));
            ViewBag.Manual = fileContents;
            if (flag.HasValue)
                ViewBag.LinkToOther = (flag.Value == 1 ? "Student" : "Faculty");
            else
                ViewBag.LinkToOther = "Faculty";


            AddFields();

            var ReturnModel = new FacultiesViewModel();
            var facultiesFiltered = db.Faculties.Where(f => f.Display.HasValue && f.Display.Value &&
                                                ((f.FacultyApplicationDate.HasValue
                                                && EntityFunctions.TruncateTime(f.FacultyApplicationDate.Value) >=
                                                EntityFunctions.TruncateTime(DateTime.Now))
                                                || (f.StudentApplicationDate.HasValue
                                                  && EntityFunctions.TruncateTime(f.StudentApplicationDate.Value) >=
                                                  EntityFunctions.TruncateTime(DateTime.Now)))).ToList();

            if (flag.HasValue)
            {
                if (!String.IsNullOrEmpty(ViewBag.LinkToOther.ToString()) && ViewBag.LinkToOther.ToString().Equals("Student"))
                {
                    ViewBag.ColorF = "lightblue";
                    var facultiesTeachersFiltered = facultiesFiltered.Where(f => (f.IsFeatured.HasValue && f.IsFeatured.Value) &&
                                                        f.FacultyPlacesAvailable.HasValue && f.FacultyPlacesAvailable.Value > 0 &&
                                            f.FacultyApplicationDate.HasValue
                                            && f.FacultyApplicationDate.Value.Year >= DateTime.Now.Year
                                            && f.FacultyApplicationDate.Value.Month >= DateTime.Now.Month
                                            && f.FacultyApplicationDate.Value.Day >= DateTime.Now.Day &&
                                            f.FacultyEnrollmentDate.HasValue).ToList().OrderBy(l => l.FacultyApplicationDate.Value).Take(10);
                    if (facultiesTeachersFiltered.ToList().Count < 10)
                    {
                        var facultiesTeachersFilteredNotFeatured = facultiesFiltered.Where(f =>
                                                (!f.IsFeatured.HasValue || (f.IsFeatured.HasValue && !f.IsFeatured.Value)) &&
                                                f.FacultyPlacesAvailable.HasValue && f.FacultyPlacesAvailable.Value > 0 &&
                                            f.FacultyApplicationDate.HasValue
                                            && f.FacultyApplicationDate.Value.Year >= DateTime.Now.Year
                                            && f.FacultyApplicationDate.Value.Month >= DateTime.Now.Month
                                            && f.FacultyApplicationDate.Value.Day >= DateTime.Now.Day &&
                                            f.FacultyEnrollmentDate.HasValue).ToList().OrderBy(l => l.FacultyApplicationDate.Value).Take(10 - facultiesTeachersFiltered.ToList().Count);


                        facultiesTeachersFiltered = facultiesTeachersFiltered.Concat(facultiesTeachersFilteredNotFeatured);

                    }

                    foreach (var fax in facultiesTeachersFiltered)
                    {
                        fax.CoursesString = string.Join(", ", fax.Courses.Select(x => x.SubjectObj.Name));
                    }

                    ReturnModel.TeacherPlaces = new List<Faculty>(facultiesTeachersFiltered);
                    return View(ReturnModel);
                }

            }

            ViewBag.ColorS = "lightblue";
            var facultiesFilteredFinal = facultiesFiltered.Where(f => (f.IsFeatured.HasValue && f.IsFeatured.Value) &&
                                               f.StudentPlacesAvailable.HasValue && f.StudentPlacesAvailable.Value > 0 &&
                                                f.StudentApplicationDate.HasValue
                                                && f.StudentApplicationDate.Value.Year >= DateTime.Now.Year
                                                && f.StudentApplicationDate.Value.Month >= DateTime.Now.Month
                                                && f.StudentApplicationDate.Value.Day >= DateTime.Now.Day &&
                                                f.StudentEnrollmentDate.HasValue).ToList().OrderBy(l => l.StudentApplicationDate.Value).Take(10);
            if (facultiesFilteredFinal.ToList().Count < 10)
            {
                var facultiesStudentsFilteredNotFeatured = facultiesFiltered.Where(f =>
                                        (!f.IsFeatured.HasValue || (f.IsFeatured.HasValue && !f.IsFeatured.Value)) &&
                                        f.StudentPlacesAvailable.HasValue && f.StudentPlacesAvailable.Value > 0 &&
                                                f.StudentApplicationDate.HasValue
                                                && f.StudentApplicationDate.Value.Year >= DateTime.Now.Year
                                                && f.StudentApplicationDate.Value.Month >= DateTime.Now.Month
                                                && f.StudentApplicationDate.Value.Day >= DateTime.Now.Day &&
                                                f.StudentEnrollmentDate.HasValue).ToList().OrderBy(l => l.StudentApplicationDate.Value).Take(10 - facultiesFilteredFinal.ToList().Count);

                facultiesFilteredFinal = facultiesFilteredFinal.Concat(facultiesStudentsFilteredNotFeatured);
            }
            ReturnModel.StudentsPlaces = new List<Faculty>(facultiesFilteredFinal);

            return View(ReturnModel);

        }

        public ActionResult ListFaculties(bool? StudentSelected, bool? FacultySelected, bool? currStud, bool? currFac, string SearchProgram, string currentProgram, string SearchKeyword, string currentFilter, int? CountryId, int? currentCountry, int? RegionId, int? currentRegion, int? page)
        {
            ViewBag.CurrentFilter = string.IsNullOrEmpty(SearchKeyword) ? currentFilter : SearchKeyword;
            ViewBag.CurrentCountry = CountryId.HasValue ? CountryId : currentCountry;
            ViewBag.CurrentRegion = RegionId.HasValue ? RegionId : currentRegion;
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

            if (!RegionId.HasValue)
            {
                RegionId = currentRegion;
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

            var facultiesFiltered = db.Faculties.Where(f => f.Display.HasValue && f.Display.Value &&
                                                ((f.FacultyApplicationDate.HasValue
                                                && EntityFunctions.TruncateTime(f.FacultyApplicationDate.Value) >=
                                                EntityFunctions.TruncateTime(DateTime.Now))
                                                || (f.StudentApplicationDate.HasValue
                                                  && EntityFunctions.TruncateTime(f.StudentApplicationDate.Value) >=
                                                  EntityFunctions.TruncateTime(DateTime.Now)))).OrderByDescending(l => l.IsFeatured).ThenBy(l => l.StudentApplicationDate.Value).ThenBy(l => l.FacultyApplicationDate.Value).ToList();

            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.Name.ToLower().Contains(SearchKeyword.ToLower())).ToList();
            }
            if (CountryId.HasValue && CountryId != -1)
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.CountryId.HasValue && f.CountryId == CountryId).ToList();
            }
            if (RegionId.HasValue && RegionId != -1 && CountryId.HasValue && CountryId == -1)
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.CountryObj != null && f.CountryObj.RegionId == RegionId).ToList();
            }
            if (!string.IsNullOrEmpty(SearchProgram))
            {
                facultiesFiltered = facultiesFiltered.Where(f => f.Program.ToLower().Contains(SearchProgram.ToLower())).ToList();
            }


            //TO DO - logika so datumi za sporedba so momentalen datum
            if ((StudentSelected.HasValue && StudentSelected.Value))
            {
                facultiesFiltered = facultiesFiltered.Where(f =>
                                            (f.StudentPlacesAvailable.HasValue && f.StudentPlacesAvailable.Value > 0 &&
                                                f.StudentApplicationDate.HasValue
                                                && f.StudentApplicationDate.Value.Year >= DateTime.Now.Year
                                                && f.StudentApplicationDate.Value.Month >= DateTime.Now.Month
                                                && f.StudentApplicationDate.Value.Day >= DateTime.Now.Day &&
                                                f.StudentEnrollmentDate.HasValue &&
                                                (StudentSelected.HasValue && StudentSelected.Value))).ToList().OrderByDescending(l => l.IsFeatured).ThenBy(l => l.StudentApplicationDate.Value).ToList();
            }
            else
                if ((FacultySelected.HasValue && FacultySelected.Value))
            {
                facultiesFiltered = facultiesFiltered.Where(f => (f.FacultyPlacesAvailable.HasValue && f.FacultyPlacesAvailable.Value > 0 &&
                                            f.FacultyApplicationDate.HasValue
                                            && f.FacultyApplicationDate.Value.Year >= DateTime.Now.Year
                                            && f.FacultyApplicationDate.Value.Month >= DateTime.Now.Month
                                            && f.FacultyApplicationDate.Value.Day >= DateTime.Now.Day &&
                                            f.FacultyEnrollmentDate.HasValue &&
                                            (FacultySelected.HasValue && FacultySelected.Value))).ToList().OrderByDescending(l => l.IsFeatured).ThenBy(l => l.FacultyApplicationDate.Value).ToList();

                foreach (var fax in facultiesFiltered)
                {
                    fax.CoursesString = string.Join(", ", fax.Courses.Select(x => x.SubjectObj.Name));
                }
            }

            SearchViewModel svm = new SearchViewModel();
            svm.SearchKeyword = SearchKeyword;
            svm.CountryId = (CountryId.HasValue ? CountryId.Value : -1);
            svm.RegionId = (RegionId.HasValue ? RegionId.Value : -1);
            ViewBag.SearchViewModel = svm;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(facultiesFiltered.ToPagedList(pageNumber, pageSize));

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
            listCountries.AddRange(db.Countries.OrderBy(x => x.Name));

            ViewBag.RegionId = new SelectList(list, "ID", "Name");

            ViewBag.CountryId = new SelectList(listCountries, "ID", "Name");
        }

        [HttpGet]
        public JsonResult getCountries(int regionId)
        {
            NomCountry initialC = new NomCountry();
            initialC.ID = -1;
            initialC.Name = "Entire region";
            List<Models.NomCountry> listCountries = new List<Models.NomCountry>();
            listCountries.Add(initialC);
            listCountries.AddRange(db.Countries.Where(c => (c.RegionId.Value == regionId && regionId != -1) || regionId == -1).OrderBy(x => x.Name));
            var countries = listCountries.Select(c => new
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