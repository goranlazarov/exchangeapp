using System.ComponentModel.DataAnnotations;

namespace ExchangeApp.Models
{
    public class NomTypeOfExchange : BaseObject
    {
        [Required(ErrorMessage = "Please enter: Type of exchange")]
        [Display(Name = "Type of exchange")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}