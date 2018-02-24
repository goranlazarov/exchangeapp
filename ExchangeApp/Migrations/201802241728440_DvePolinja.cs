namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DvePolinja : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faculties", "Description", c => c.String(nullable: false, maxLength: 4000));
            AddColumn("dbo.Faculties", "AgreementNumber", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faculties", "AgreementNumber");
            DropColumn("dbo.Faculties", "Description");
        }
    }
}
