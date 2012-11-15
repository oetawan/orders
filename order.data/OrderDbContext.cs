﻿using order.data.Migrations;
using order.model;
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
        
        static OrderDbContext()
        {
            Database.SetInitializer(new OrderDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}