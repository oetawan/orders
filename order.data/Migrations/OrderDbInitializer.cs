using order.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace order.data.Migrations
{
    public class OrderDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<OrderDbContext>
    {
        protected override void Seed(OrderDbContext context)
        {
            context.Customers.Add(new Customer { LicenseId = "1234567890", CustomerCode = "MCD", CustomerName = "Mc Donald", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            context.Customers.Add(new Customer { LicenseId = "1348348748", CustomerCode = "KFC", CustomerName = "Kentucy Fried Chikend", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            context.Customers.Add(new Customer { LicenseId = "5989859477", CustomerCode = "PIZ", CustomerName = "Pizza Hut", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            context.Customers.Add(new Customer { LicenseId = "4938493889", CustomerCode = "AWE", CustomerName = "A.W", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });
            context.Customers.Add(new Customer { LicenseId = "9898434389", CustomerCode = "SBC", CustomerName = "Starbuck Coffee", Registered = false, ServiceBusNamespace = "zainorder", Issuer = "owner", SecretKey = "01NCCo0nGzshAWfZymfA0ES8f8v9Qwx4U/JqlR68ApM=" });

            context.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_LicenseId]
                                                 ON [dbo].[Customers]([LicenseId] ASC);");
            context.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_CustomerCode]
                                                 ON [dbo].[Customers]([CustomerCode] ASC);");
            context.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_Branches_Username]
                                                 ON [dbo].[Branches]([Username] ASC);");
            context.Database.ExecuteSqlCommand(@"CREATE UNIQUE NONCLUSTERED INDEX [IX_ShoppingCartStores_UserId] 
                                                 ON [dbo].[ShoppingCartStores]([UserId] ASC);");
        }
    }
}