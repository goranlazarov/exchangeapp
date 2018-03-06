using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class FacultiesViewModel
    {
        public IEnumerable<Faculty> StudentsPlaces { get; set; }

        public IEnumerable<Faculty> TeacherPlaces { get; set; }

    }
}