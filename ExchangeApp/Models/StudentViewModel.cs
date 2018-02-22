using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class StudentViewModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Nationality { get; set; }

        public string CountryOfOrigin { get; set; }

        public DateTime DateOfEnrollment { get; set; }

        public DateTime DateOfCompletion { get; set; }

        public string ProgramEnrolled { get; set; }

        public string SemesterEnrolled { get; set; }

        public string EnglishLevel { get; set; }

        public string SchoolName { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public int AgreementNumber { get; set; }

        public int AccreditationNumber { get; set; }

    }
}