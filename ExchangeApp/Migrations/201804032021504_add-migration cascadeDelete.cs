namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmigrationcascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions");
            DropForeignKey("dbo.Subjects", "FacultyId", "dbo.Faculties");
            AddForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Subjects", "FacultyId", "dbo.Faculties", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subjects", "FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions");
            AddForeignKey("dbo.Subjects", "FacultyId", "dbo.Faculties", "ID");
            AddForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions", "ID");
        }
    }
}
