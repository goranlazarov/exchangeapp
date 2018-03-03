using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class Semester : BaseObject
    {
        [Required(ErrorMessage = "Please enter description")]
        [Display(Name = "Semester")]
        [StringLength(50)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please choose school year")]
        [Display(Name = "School year")]
        public int? SchoolYearId { get; set; }

        [Display(Name = "School year")]
        [ForeignKey("SchoolYearId")]
        public virtual NomSchoolYear SchoolYearObj { get; set; }
    }
}