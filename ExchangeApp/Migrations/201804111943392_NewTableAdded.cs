namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacultyCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FacultyId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Faculties", t => t.FacultyId)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .ForeignKey("dbo.Subjects", t => t.SubjectId)
                .Index(t => t.FacultyId)
                .Index(t => t.SubjectId)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FacultyCourses", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.FacultyCourses", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.FacultyCourses", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.FacultyCourses", "FacultyId", "dbo.Faculties");
            DropIndex("dbo.FacultyCourses", new[] { "LastUpdatedBy" });
            DropIndex("dbo.FacultyCourses", new[] { "RegisteredBy" });
            DropIndex("dbo.FacultyCourses", new[] { "SubjectId" });
            DropIndex("dbo.FacultyCourses", new[] { "FacultyId" });
            DropTable("dbo.FacultyCourses");
        }
    }
}
