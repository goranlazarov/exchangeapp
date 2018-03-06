using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class NomApplicantHighestDegree : BaseObject
    {
        [Required(ErrorMessage = "Please enter applicant highest degree")]
        [Display(Name = "Applicant degree")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}