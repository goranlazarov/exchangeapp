namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisplayColumnFaculties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faculties", "Display", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faculties", "Display");
        }
    }
}
