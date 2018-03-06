using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class NomSchoolYear: BaseObject
    {
        [Required(ErrorMessage = "Please enter school year")]
        [Display(Name = "School year")]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Semester> Semesters { get; set; }

    }
}