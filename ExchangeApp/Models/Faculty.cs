using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class Faculty : BaseObject
    {
        [Required(ErrorMessage = "Please enter: School name")]
        [Display(Name = "School name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter: School program")]
        [Display(Name = "School program")]
        [StringLength(50)]
        public string Program { get; set; }

        [Required(ErrorMessage = "Please enter: Email")]
        [Display(Name = "Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter: Description")]
        [Display(Name = "Description")]
        [StringLength(4000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter: Agreement number")]
        [Display(Name = "Agreement number")]
        [StringLength(20)]
        public string AgreementNumber { get; set; }

        [Required(ErrorMessage = "Please enter: Website")]
        [Display(Name = "Website")]
        [StringLength(50)]
        public string Website { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        [Display(Name = "Country")]
        [ForeignKey("CountryId")]
        public virtual NomCountry CountryObj { get; set; }

        [Required(ErrorMessage = "Please enter: Date of Matriculation")]
        [Display(Name = "Date of Matriculation")]
        public DateTime? DateOfMatriculation { get; set; }

        [Required(ErrorMessage = "Please enter: Accreditation number")]
        [Display(Name = "Accreditation number")]
        public int AccreditationNumber { get; set; }

        [Required(ErrorMessage = "Please enter: Date of Accreditation")]
        [Display(Name = "Date of Accreditation")]
        public DateTime? DateOfAccreditation { get; set; }

        [Display(Name = "Student places available for exchange")]
        public int? StudentPlacesAvailable { get; set; }

        [Display(Name = "Student application date")]
        public DateTime? StudentApplicationDate { get; set; }

        [Display(Name = "Student enrollment date")]
        public DateTime? StudentEnrollmentDate { get; set; }

        [Display(Name = "Faculty places available for exchange")]
        public int? FacultyPlacesAvailable { get; set; }

        [Display(Name = "Faculty application date")]
        public DateTime? FacultyApplicationDate { get; set; }

        [Display(Name = "Faculty enrollment date")]
        public DateTime? FacultyEnrollmentDate { get; set; }

        [Display(Name = "Student type of exchange")]
        public int? StudentTypeOfExchangeId { get; set; }

        [Display(Name = "Student type of exchange")]
        [ForeignKey("StudentTypeOfExchangeId")]
        public virtual NomTypeOfExchange StudentTypeOfExchangeObj { get; set; }

        [Display(Name = "Faculty type of exchange")]
        public int? FacultyTypeOfExchangeId { get; set; }

        [Display(Name = "Faculty type of exchange")]
        [ForeignKey("FacultyTypeOfExchangeId")]
        public virtual NomTypeOfExchange FacultyTypeOfExchangeObj { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}