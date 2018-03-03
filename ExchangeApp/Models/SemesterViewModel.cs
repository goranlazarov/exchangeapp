namespace ExchangeApp.Models
{
    public class SemesterViewModel
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public int? SchoolYearId { get; set; }

        public virtual NomSchoolYear SchoolYearObj { get; set; }

    }
}