﻿using order.data.contract;
using order.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.data
{
    public class OrderUow : IOrderUow
    {
        private OrderDbContext DbContext { get; set; }
        public OrderUow()
        {
            CreateDbContext();
            CreateRepositories();
        }

        private void CreateDbContext()
        {
            this.DbContext = new OrderDbContext();
            this.DbContext.Configuration.ProxyCreationEnabled = false;
            this.DbContext.Configuration.LazyLoadingEnabled = false;
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        private void CreateRepositories()
        {
            this.Customers = new EFRepository<Customer>(this.DbContext);
        }

        public IRepository<Customer> Customers { get; private set; }

        public void Commit()
        {
            this.DbContext.SaveChanges();
        }
    }
}