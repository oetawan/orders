using order.data.contract;
using order.model;
using order.service.contract;
using order.snapshot;
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

        public void ChangeQty(ShoppingCart.ChangeQtyCommand cmd)
        {
            cmdExecutor.Execute(uow =>
            {
                ShoppingCart sc = uow.ShoppingCarts.Get(cmd.Username);
                sc.ChangeQty(cmd);
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

        public void CheckoutOut(ShoppingCart.CheckoutCommand cmd, Action beforeAction)
        {
            cmdExecutor.Execute(uow => {
                if (beforeAction != null)
                {
                    beforeAction();
                }

                ShoppingCart sc = uow.ShoppingCarts.Get(cmd.Username);
                if (sc == null) return;

                ShoppingCartSnapshot snapshot = sc.CreateSnapshot();

                List<OrderItem> items = new List<OrderItem>();
                Order order = new Order
                {
                    BranchId = cmd.BranchId,
                    OrderDate = cmd.OrderDate,
                    OrderDateString = cmd.OrderDate.ToString("dd-MM-yyyy"),
                    OrderNumber = cmd.OrderNumber,
                    UserId = cmd.Username,
                    TotalAmountAfterDiscount = snapshot.TotalAmountAfterDiscount,
                    Items = items
                };

                snapshot.Items.ForEach(itemSnapshot => {
                    items.Add(new OrderItem
                    {
                        Order = order,
                        ItemId = itemSnapshot.ItemId,
                        ItemCode = itemSnapshot.ItemCode,
                        ItemName = itemSnapshot.ItemName,
                        UnitCode = itemSnapshot.UnitCode,
                        Price = itemSnapshot.Price,
                        Qty = itemSnapshot.Qty,
                        AmountAfterDiscount = itemSnapshot.AmountAfterDiscount
                    });
                });

                sc.Clear();

                uow.Orders.Add(order);
                uow.ShoppingCarts.Save(sc);
            });
        }
    }
}