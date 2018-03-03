using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExchangeApp.Models
{
    public class NomRegion : BaseObject
    {
        [Required(ErrorMessage = "Please enter region name")]
        [Display(Name = "Region")]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<NomCountry> Countries { get; set; }

    }
}