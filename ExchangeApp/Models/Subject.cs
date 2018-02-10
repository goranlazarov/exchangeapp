using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class Subject : BaseObject
    {
        [Required(ErrorMessage = "Please enter: Degree Level name")]
        [Display(Name = "Degree level name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Faculty")]
        public int? FacultyId { get; set; }

        [Required]
        [Display(Name = "Degree level")]
        public int? DegreeLevelId { get; set; }

        [Display(Name = "Faculty")]
        [ForeignKey("FacultyId")]
        public virtual Faculty FacultyObj { get; set; }

        [Display(Name = "Degree level")]
        [ForeignKey("DegreeLevelId")]
        public virtual NomDegreeLevel DegreeLevelObj { get; set; }



    }
}