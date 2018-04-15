namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypesOfExchangeColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NomTypeOfExchanges", "Faculty", c => c.Boolean());
            AddColumn("dbo.NomTypeOfExchanges", "Student", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NomTypeOfExchanges", "Student");
            DropColumn("dbo.NomTypeOfExchanges", "Faculty");
        }
    }
}
