namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faculties", "LogoImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faculties", "LogoImage");
        }
    }
}
