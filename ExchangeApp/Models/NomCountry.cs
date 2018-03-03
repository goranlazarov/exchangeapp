using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeApp.Models
{
    public class NomCountry : BaseObject
    {
        [Required(ErrorMessage = "Please enter country name")]
        [Display(Name = "Country")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please choose region")]
        [Display(Name = "Region")]
        public int? RegionId { get; set; }

        [Display(Name = "Region")]
        [ForeignKey("RegionId")]
        public virtual NomRegion RegionObj { get; set; }

        public virtual ICollection<Faculty> Faculties { get; set; }
    }
}