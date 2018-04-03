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
        [Required(ErrorMessage = "Please enter semester name")]
        [Display(Name = "Semester")]
        [StringLength(50)]
        public string Description { get; set; }

    }
}