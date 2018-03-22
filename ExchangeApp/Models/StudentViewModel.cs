using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class StudentViewModel
    {
        public Faculty Faculty { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Nationality { get; set; }

        public string CountryOfOrigin { get; set; }

        public HttpPostedFileBase CV { get; set; }

        public string YearOfEnrollment { get; set; }

        public string YearOfCompletion { get; set; }

        public string AgreementNumber { get; set; }

        public string ProgramEnrolled { get; set; }

        public string SemesterEnrolled { get; set; }

        public string EnglishLevel { get; set; }

        public string HighestDegree { get; set; }

        public string FirstCourse { get; set; }

        public string SecondCourse { get; set; }

        public string ThirdCourse { get; set; }

        public string FourthCourse { get; set; }

        public bool StudentSelected { get; set; }

        public bool FacultySelected { get; set; }


        public StudentViewModel(Faculty faculty)
        {
            this.Faculty = faculty;
        }

        public StudentViewModel()
        {

        }

    }
}