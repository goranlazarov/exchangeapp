using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using imis.CustomValidations;
namespace ExchangeApp.Models
{
    public class NomDegreeLevel : BaseObject
    {
        [Required(ErrorMessage = "Please enter degree level")]
        [Display(Name = "Degree level")]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}