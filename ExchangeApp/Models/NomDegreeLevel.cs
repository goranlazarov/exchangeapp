using System.ComponentModel.DataAnnotations;
//using imis.CustomValidations;
namespace ExchangeApp.Models
{

    public class NomDegreeLevel : BaseObject
    {
        [Required(ErrorMessage = "Please enter: Degree Level name")]
        [Display(Name = "Degree level name")]
        [StringLength(50)]
        public string Name { get; set; }

    }
}