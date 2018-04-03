namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubjectCleaned : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions");
            DropIndex("dbo.Subjects", new[] { "DegreeLevelId" });
            RenameColumn(table: "dbo.Subjects", name: "DegreeLevelId", newName: "NomDegreeLevel_ID");
            AlterColumn("dbo.Subjects", "NomDegreeLevel_ID", c => c.Int());
            CreateIndex("dbo.Subjects", "NomDegreeLevel_ID");
            AddForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions");
            DropIndex("dbo.Subjects", new[] { "NomDegreeLevel_ID" });
            AlterColumn("dbo.Subjects", "NomDegreeLevel_ID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Subjects", name: "NomDegreeLevel_ID", newName: "DegreeLevelId");
            CreateIndex("dbo.Subjects", "DegreeLevelId");
            AddForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions", "ID", cascadeDelete: true);
        }
    }
}
