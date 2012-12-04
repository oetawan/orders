using order.data.contract;
using order.model;
using order.service.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.service
{
    public class OrderService: IOrderService
    {
        private CommandExecutor cmdExecutor;
        public OrderService(IOrderUow orderUow)
        {
            this.cmdExecutor = new CommandExecutor(orderUow);
        }

        public void AddItemToOrder(ShoppingCart.AddItemCommand cmd)
        {
            cmdExecutor.Execute(uow => {
                ShoppingCart sc = uow.ShoppingCarts.Get(cmd.Username);
                sc.AddItem(cmd);
                uow.ShoppingCarts.Save(sc);
            });
        }

        public void RemoveItem(ShoppingCart.RemoveItemCommand cmd)
        {
            cmdExecutor.Execute(uow => {
                ShoppingCart sc = uow.ShoppingCarts.Get(cmd.Username);
                sc.RemoveItem(cmd);
                uow.ShoppingCarts.Save(sc);
            });
        }
    }
}