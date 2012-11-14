namespace order.data.Migrations
{
    using order.model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<order.data.OrderDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(order.data.OrderDbContext context)
        {
            context.Customers.AddOrUpdate(cs => cs.LicenseId,
                new Customer { LicenseId = "1234567890", BranchCode = "BTM", CustomerCode = "MCD", Name = "Mc Donald", Registered = false },
                new Customer { LicenseId = "1348348748", BranchCode = "BAL", CustomerCode = "MCD", Name = "Mc Donald", Registered = false },
                new Customer { LicenseId = "5989859477", BranchCode = "BDG", CustomerCode = "MCD", Name = "Mc Donald", Registered = false },
                new Customer { LicenseId = "4938493889", BranchCode = "SBY", CustomerCode = "MCD", Name = "Mc Donald", Registered = false },
                new Customer { LicenseId = "9898434389", BranchCode = "MDN", CustomerCode = "MCD", Name = "Mc Donald", Registered = false });
           
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
