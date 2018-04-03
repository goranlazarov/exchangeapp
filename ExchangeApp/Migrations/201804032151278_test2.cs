namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subjects", "NomDegreeLevel_ID", "dbo.NomDegreeLevels");
            DropIndex("dbo.Subjects", new[] { "NomDegreeLevel_ID" });
            DropColumn("dbo.Subjects", "NomDegreeLevel_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Subjects", "NomDegreeLevel_ID", c => c.Int());
            CreateIndex("dbo.Subjects", "NomDegreeLevel_ID");
            AddForeignKey("dbo.Subjects", "NomDegreeLevel_ID", "dbo.NomDegreeLevels", "ID");
        }
    }
}
