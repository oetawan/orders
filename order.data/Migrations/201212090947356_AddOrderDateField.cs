namespace order.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderDateField : DbMigration
    {
        public override void Up()
        {
            AddColumn("Orders", "OrderDate", cb => cb.DateTime(), null);
        }
        
        public override void Down()
        {
            DropColumn("Orders", "OrderDate", null);
        }
    }
}