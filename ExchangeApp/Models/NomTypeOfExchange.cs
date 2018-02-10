using System.ComponentModel.DataAnnotations;

namespace ExchangeApp.Models
{
    public class NomTypeOfExchange : BaseObject
    {
        [Required(ErrorMessage = "Please enter: Type of exchange name")]
        [Display(Name = "Type of exchange name")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}