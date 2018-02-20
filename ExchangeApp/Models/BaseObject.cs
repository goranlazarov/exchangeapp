using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExchangeApp.Models
{
    public class BaseObject
    {
        public BaseObject()
        {
            Registered = Helpers.helpers.GlobalSystemTime();
            LastUpdated = Registered;
            RegisteredBy = "8dde236e-1b0a-4e32-871a-7833696d74bf"; //Helpers.helpers.GetCurrentUserID();
            LastUpdatedBy = RegisteredBy;
        }

        /*
         * Purpose : setups moment of last updated and last updated users during model object updating
         */
        public void Access()
        {
            LastUpdated = Helpers.helpers.GlobalSystemTime();
            //if (!string.IsNullOrEmpty(Helpers.helpers.GetCurrentUserID()))
                LastUpdatedBy = "8dde236e-1b0a-4e32-871a-7833696d74bf"; // Helpers.helpers.GetCurrentUserID();
        }

        [Required(ErrorMessage = "Please enter : ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter : Registered")]
        [Display(Name = "Registered")]
        public DateTime? Registered { get; set; }

        [Required(ErrorMessage = "Please enter : RegisteredBy")]
        [Display(Name = "Registered By")]
        [StringLength(128)]
        public string RegisteredBy { get; set; }

        [Required(ErrorMessage = "Please enter : LastUpdated")]
        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [Required(ErrorMessage = "Please enter : LastUpdatedBy")]
        [Display(Name = "Last Updated By")]
        [StringLength(128)]
        public string LastUpdatedBy { get; set; }


        [Display(Name = "Registered By")]
        [ForeignKey("RegisteredBy")]
        public virtual ApplicationUser RegisteredByUser { get; set; }

        [ForeignKey("LastUpdatedBy")]
        [Display(Name = "Last Updated By")]
        public virtual ApplicationUser LastUpdatedByUser { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    
}