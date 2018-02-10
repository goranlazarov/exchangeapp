namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class all_models_10022018 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NomApplicantHighestDegrees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
            CreateTable(
                "dbo.NomCountries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        RegionId = c.Int(nullable: false),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.NomRegions", t => t.RegionId)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .Index(t => t.RegionId)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Program = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Website = c.String(nullable: false, maxLength: 50),
                        CountryId = c.Int(nullable: false),
                        DateOfMatriculation = c.DateTime(nullable: false),
                        AccreditationNumber = c.Int(nullable: false),
                        DateOfAccreditation = c.DateTime(nullable: false),
                        StudentPlacesAvailable = c.Int(),
                        StudentApplicationDate = c.DateTime(),
                        StudentEnrollmentDate = c.DateTime(),
                        FacultyPlacesAvailable = c.Int(),
                        FacultyApplicationDate = c.DateTime(),
                        FacultyEnrollmentDate = c.DateTime(),
                        StudentTypeOfExchangeId = c.Int(),
                        FacultyTypeOfExchangeId = c.Int(),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NomCountries", t => t.CountryId)
                .ForeignKey("dbo.NomTypeOfExchanges", t => t.FacultyTypeOfExchangeId)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .ForeignKey("dbo.NomTypeOfExchanges", t => t.StudentTypeOfExchangeId)
                .Index(t => t.CountryId)
                .Index(t => t.StudentTypeOfExchangeId)
                .Index(t => t.FacultyTypeOfExchangeId)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        FacultyId = c.Int(nullable: false),
                        DegreeLevelId = c.Int(nullable: false),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NomDegreeLevels", t => t.DegreeLevelId)
                .ForeignKey("dbo.Faculties", t => t.FacultyId)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .Index(t => t.FacultyId)
                .Index(t => t.DegreeLevelId)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
            CreateTable(
                "dbo.NomRegions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
            CreateTable(
                "dbo.NomEnglishLevels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
            CreateTable(
                "dbo.NomNationalities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
            CreateTable(
                "dbo.NomSchoolYears",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        SchoolYearId = c.Int(nullable: false),
                        Registered = c.DateTime(nullable: false),
                        RegisteredBy = c.String(nullable: false, maxLength: 128),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.RegisteredBy)
                .ForeignKey("dbo.NomSchoolYears", t => t.SchoolYearId)
                .Index(t => t.SchoolYearId)
                .Index(t => t.RegisteredBy)
                .Index(t => t.LastUpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Semesters", "SchoolYearId", "dbo.NomSchoolYears");
            DropForeignKey("dbo.Semesters", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Semesters", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomSchoolYears", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomSchoolYears", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomNationalities", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomNationalities", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomEnglishLevels", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomEnglishLevels", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomCountries", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomRegions", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomRegions", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomCountries", "RegionId", "dbo.NomRegions");
            DropForeignKey("dbo.NomCountries", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subjects", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subjects", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subjects", "FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.Subjects", "DegreeLevelId", "dbo.NomDegreeLevels");
            DropForeignKey("dbo.Faculties", "StudentTypeOfExchangeId", "dbo.NomTypeOfExchanges");
            DropForeignKey("dbo.Faculties", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Faculties", "LastUpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Faculties", "FacultyTypeOfExchangeId", "dbo.NomTypeOfExchanges");
            DropForeignKey("dbo.Faculties", "CountryId", "dbo.NomCountries");
            DropForeignKey("dbo.NomApplicantHighestDegrees", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomApplicantHighestDegrees", "LastUpdatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Semesters", new[] { "LastUpdatedBy" });
            DropIndex("dbo.Semesters", new[] { "RegisteredBy" });
            DropIndex("dbo.Semesters", new[] { "SchoolYearId" });
            DropIndex("dbo.NomSchoolYears", new[] { "LastUpdatedBy" });
            DropIndex("dbo.NomSchoolYears", new[] { "RegisteredBy" });
            DropIndex("dbo.NomNationalities", new[] { "LastUpdatedBy" });
            DropIndex("dbo.NomNationalities", new[] { "RegisteredBy" });
            DropIndex("dbo.NomEnglishLevels", new[] { "LastUpdatedBy" });
            DropIndex("dbo.NomEnglishLevels", new[] { "RegisteredBy" });
            DropIndex("dbo.NomRegions", new[] { "LastUpdatedBy" });
            DropIndex("dbo.NomRegions", new[] { "RegisteredBy" });
            DropIndex("dbo.Subjects", new[] { "LastUpdatedBy" });
            DropIndex("dbo.Subjects", new[] { "RegisteredBy" });
            DropIndex("dbo.Subjects", new[] { "DegreeLevelId" });
            DropIndex("dbo.Subjects", new[] { "FacultyId" });
            DropIndex("dbo.Faculties", new[] { "LastUpdatedBy" });
            DropIndex("dbo.Faculties", new[] { "RegisteredBy" });
            DropIndex("dbo.Faculties", new[] { "FacultyTypeOfExchangeId" });
            DropIndex("dbo.Faculties", new[] { "StudentTypeOfExchangeId" });
            DropIndex("dbo.Faculties", new[] { "CountryId" });
            DropIndex("dbo.NomCountries", new[] { "LastUpdatedBy" });
            DropIndex("dbo.NomCountries", new[] { "RegisteredBy" });
            DropIndex("dbo.NomCountries", new[] { "RegionId" });
            DropIndex("dbo.NomApplicantHighestDegrees", new[] { "LastUpdatedBy" });
            DropIndex("dbo.NomApplicantHighestDegrees", new[] { "RegisteredBy" });
            DropTable("dbo.Semesters");
            DropTable("dbo.NomSchoolYears");
            DropTable("dbo.NomNationalities");
            DropTable("dbo.NomEnglishLevels");
            DropTable("dbo.NomRegions");
            DropTable("dbo.Subjects");
            DropTable("dbo.Faculties");
            DropTable("dbo.NomCountries");
            DropTable("dbo.NomApplicantHighestDegrees");
        }
    }
}
