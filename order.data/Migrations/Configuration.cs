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
            if (context.Customers.Where(cs => cs.LicenseId == "1234567890").Count() == 0)
            {
                context.Customers.Add(new Customer { LicenseId = "1234567890", CustomerCode = "MCD", CustomerName = "Mc Donald", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            }

            if (context.Customers.Where(cs => cs.LicenseId == "1348348748").Count() == 0)
            {
                context.Customers.Add(new Customer { LicenseId = "1348348748", CustomerCode = "KFC", CustomerName = "Kentucy Fried Chikend", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            }

            if (context.Customers.Where(cs => cs.LicenseId == "5989859477").Count() == 0)
            {
                context.Customers.Add(new Customer { LicenseId = "5989859477", CustomerCode = "PIZ", CustomerName = "Pizza Hut", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            }

            if (context.Customers.Where(cs => cs.LicenseId == "4938493889").Count() == 0)
            {
                context.Customers.Add(new Customer { LicenseId = "4938493889", CustomerCode = "AWE", CustomerName = "A.W", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            }

            if (context.Customers.Where(cs => cs.LicenseId == "9898434389").Count() == 0)
            {
                context.Customers.Add(new Customer { LicenseId = "9898434389", CustomerCode = "SBC", CustomerName = "Starbuck Coffee", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            }
            
            try
            {
                context.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_LicenseId]
                                                 ON [dbo].[Customers]([LicenseId] ASC);");
            }
            catch { }
            
            try
            {
                context.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_CustomerCode]
                                                 ON [dbo].[Customers]([CustomerCode] ASC);");
            }
            catch { }

            try
            {
                context.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_Branches_Username]
                                                 ON [dbo].[Branches]([Username] ASC);");
            }
            catch { }

            try
            {
                context.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_ShoppingCartStores_UserId] 
                                                 ON [dbo].[ShoppingCartStores]([UserId] ASC);");
            }
            catch { }
        }
    }
}