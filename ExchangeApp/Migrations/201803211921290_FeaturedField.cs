namespace ExchangeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeaturedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Faculties", "IsFeatured", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Faculties", "IsFeatured");
        }
    }
}
