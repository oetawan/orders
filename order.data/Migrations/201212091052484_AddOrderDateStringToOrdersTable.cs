namespace order.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderDateStringToOrdersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("Orders", "OrderDateString", cb => cb.String(), null);
        }

        public override void Down()
        {
            DropColumn("Orders", "OrderDateString", null);
        }
    }
}