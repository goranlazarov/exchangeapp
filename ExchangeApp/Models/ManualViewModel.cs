﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExchangeApp.Models
{
    public class ManualViewModel
    {
        [AllowHtml]
        public string ManualText { get; set; }
    }
}