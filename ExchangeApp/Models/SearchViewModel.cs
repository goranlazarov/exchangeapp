using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class SearchViewModel
    {

        public string SearchKeyword { get; set; }
        public int CountryId { get; set; }

    }
}