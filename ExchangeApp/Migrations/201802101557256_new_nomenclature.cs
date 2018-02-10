namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new_nomenclature : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NomTypeOfExchanges",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NomTypeOfExchanges", "RegisteredBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NomTypeOfExchanges", "LastUpdatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.NomTypeOfExchanges", new[] { "LastUpdatedBy" });
            DropIndex("dbo.NomTypeOfExchanges", new[] { "RegisteredBy" });
            DropTable("dbo.NomTypeOfExchanges");
        }
    }
}
