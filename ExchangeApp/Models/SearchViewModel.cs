using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class SearchViewModel
    {

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Nationality { get; set; }

        public string CountryOfOrigin { get; set; }

        public string ApplicantHighestDegree { get; set; }

        public string EnglishLevel { get; set; }

        public string SchoolName { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string FirstSubject { get; set; }

        public string SecondSubject { get; set; }

        public string ThirdSubject { get; set; }

        public string FourthSubject { get; set; }

        public int AgreementNumber { get; set; }

        public int AccreditationNumber { get; set; }


    }
}