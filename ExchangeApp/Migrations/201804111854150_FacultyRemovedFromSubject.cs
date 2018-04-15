namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacultyRemovedFromSubject : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subjects", "FacultyId", "dbo.Faculties");
            DropIndex("dbo.Subjects", new[] { "FacultyId" });
            RenameColumn(table: "dbo.Subjects", name: "FacultyId", newName: "Faculty_ID");
            AlterColumn("dbo.Subjects", "Faculty_ID", c => c.Int());
            CreateIndex("dbo.Subjects", "Faculty_ID");
            AddForeignKey("dbo.Subjects", "Faculty_ID", "dbo.Faculties", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subjects", "Faculty_ID", "dbo.Faculties");
            DropIndex("dbo.Subjects", new[] { "Faculty_ID" });
            AlterColumn("dbo.Subjects", "Faculty_ID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Subjects", name: "Faculty_ID", newName: "FacultyId");
            CreateIndex("dbo.Subjects", "FacultyId");
            AddForeignKey("dbo.Subjects", "FacultyId", "dbo.Faculties", "ID", cascadeDelete: true);
        }
    }
}
