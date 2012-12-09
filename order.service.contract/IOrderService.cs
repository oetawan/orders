using order.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace order.service.contract
{
    public interface IOrderService
    {
        void AddItemToOrder(ShoppingCart.AddItemCommand cmd);
        void ChangeQty(ShoppingCart.ChangeQtyCommand cmd);
        void RemoveItem(ShoppingCart.RemoveItemCommand cmd);
        void CheckoutOut(ShoppingCart.CheckoutCommand cmd, Action beforeAction);
    }
}