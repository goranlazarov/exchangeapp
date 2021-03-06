﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class Subject : BaseObject
    {
        [Required(ErrorMessage = "Please enter subject name")]
        [Display(Name = "Subject name")]
        [StringLength(50)]
        public string Name { get; set; }

    }
}