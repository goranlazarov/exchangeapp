using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class NomNationality : BaseObject
    {
        [Required(ErrorMessage = "Please enter: Natonality name")]
        [Display(Name = "Nationality")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}