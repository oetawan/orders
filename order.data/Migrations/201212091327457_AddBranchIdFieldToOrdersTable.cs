namespace order.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBranchIdFieldToOrdersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("Orders", "BranchId", cb => cb.Int(), null);
        }

        public override void Down()
        {
            DropColumn("Orders", "BranchId", null);
        }
    }
}