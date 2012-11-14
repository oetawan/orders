namespace order.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegisteredColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Registered", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Registered");
        }
    }
}