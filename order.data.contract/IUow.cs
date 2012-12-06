using order.model;
using order.snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace order.data.contract
{
    public interface IOrderUow
    {
        IRepository<Customer> Customers { get; }
        IRepository<UserCustomerMapping> UserCustomerMapping { get; }
        IRepository<Branch> Branches { get; }
        IShoppingCartRepository ShoppingCarts { get; }
        IRepository<Order> Orders { get; }
        void Commit();
    }
}