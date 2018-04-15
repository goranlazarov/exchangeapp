using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeApp.Models
{
    public class FacultyCourses : BaseObject
    {
        [Required(ErrorMessage = "Please choose faculty")]
        [Display(Name = "Faculty")]
        public int? FacultyId { get; set; }

        [Required(ErrorMessage = "Please choose subject")]
        [Display(Name = "Subject")]
        public int? SubjectId { get; set; }

        [Display(Name = "Faculty")]
        [ForeignKey("FacultyId")]
        public virtual Faculty FacultyObj { get; set; }

        [Display(Name = "Subject")]
        [ForeignKey("SubjectId")]
        public virtual Subject SubjectObj { get; set; }



    }
}