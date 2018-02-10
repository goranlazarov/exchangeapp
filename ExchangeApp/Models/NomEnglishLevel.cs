using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class NomEnglishLevel : BaseObject
    {
        [Required(ErrorMessage = "Please enter: English proeficiency")]
        [Display(Name = "English proeficiency")]
        [StringLength(50)]
        public string Name { get; set; }
    }
}