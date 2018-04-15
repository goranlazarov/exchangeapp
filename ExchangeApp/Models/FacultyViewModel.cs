using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class FacultyViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Program { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string AgreementNumber { get; set; }

        public string Website { get; set; }

        public int? CountryId { get; set; }

        public virtual NomCountry CountryObj { get; set; }

        public DateTime? DateOfMatriculation { get; set; }

        public int AccreditationNumber { get; set; }

        public DateTime? DateOfAccreditation { get; set; }

        public int? StudentPlacesAvailable { get; set; }

        public DateTime? StudentApplicationDate { get; set; }

        public DateTime? StudentEnrollmentDate { get; set; }

        public int? FacultyPlacesAvailable { get; set; }

        public DateTime? FacultyApplicationDate { get; set; }

        public DateTime? FacultyEnrollmentDate { get; set; }

        public int? StudentTypeOfExchangeId { get; set; }

        public virtual NomTypeOfExchange StudentTypeOfExchangeObj { get; set; }

        public int? FacultyTypeOfExchangeId { get; set; }

        public virtual NomTypeOfExchange FacultyTypeOfExchangeObj { get; set; }

        public bool? IsFeatured { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public byte[] LogoImage { get; set; }

        public FacultyViewModel(Faculty faculty)
        {
            this.ID = faculty.ID;
            this.Name = faculty.Name;
            this.Program = faculty.Program;
            this.Email = faculty.Email;
            this.Description = faculty.Description;
            this.AgreementNumber = faculty.AgreementNumber;
            this.Website = faculty.Website;
            this.CountryId = faculty.CountryId;
            this.CountryObj = faculty.CountryObj;
            this.DateOfAccreditation = faculty.DateOfAccreditation;
            this.DateOfMatriculation = faculty.DateOfMatriculation;
            this.AccreditationNumber = faculty.AccreditationNumber;
            this.StudentApplicationDate = faculty.StudentApplicationDate;
            this.StudentEnrollmentDate = faculty.StudentEnrollmentDate;
            this.StudentPlacesAvailable = faculty.StudentPlacesAvailable;
            this.StudentTypeOfExchangeId = faculty.StudentTypeOfExchangeId;
            this.StudentTypeOfExchangeObj = faculty.StudentTypeOfExchangeObj;
            this.FacultyApplicationDate = faculty.FacultyApplicationDate;
            this.FacultyEnrollmentDate = faculty.FacultyEnrollmentDate;
            this.FacultyPlacesAvailable = faculty.FacultyPlacesAvailable;
            this.FacultyTypeOfExchangeId = faculty.FacultyTypeOfExchangeId;
            this.FacultyTypeOfExchangeObj = faculty.FacultyTypeOfExchangeObj;
            this.IsFeatured = faculty.IsFeatured;
            this.LogoImage = faculty.LogoImage;
            
    }

        public FacultyViewModel()
        {

        }
    }
}