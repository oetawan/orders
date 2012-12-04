using order.model;
using order.snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.data.contract
{
    public interface IShoppingCartRepository
    {
        ShoppingCart Get(string username);
        void Save(ShoppingCart sc);
    }
}