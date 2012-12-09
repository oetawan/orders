namespace order.data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaeateOrderNumberTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Branch = c.String(),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderNumbers");
        }
    }
}