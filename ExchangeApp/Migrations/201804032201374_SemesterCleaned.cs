namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SemesterCleaned : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Semesters", "SchoolYearId", "dbo.NomSchoolYears");
            DropIndex("dbo.Semesters", new[] { "SchoolYearId" });
            DropColumn("dbo.Semesters", "SchoolYearId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Semesters", "SchoolYearId", c => c.Int(nullable: false));
            CreateIndex("dbo.Semesters", "SchoolYearId");
            AddForeignKey("dbo.Semesters", "SchoolYearId", "dbo.NomSchoolYears", "ID");
        }
    }
}
