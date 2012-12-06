using order.data.Configuration;
using order.data.Migrations;
using order.model;
using order.snapshot;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext()
            : base(nameOrConnectionString: "Orders")
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserCustomerMapping> UserCustomerMapping { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ShoppingCartStore> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        static OrderDbContext()
        {
            Database.SetInitializer(new OrderDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add<Order>(new OrderConfiguration());           
        }
    }
}