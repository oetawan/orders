using order.data.contract;
using order.model;
using order.snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Transactions;
namespace order.data
{
    public class ShoppingCartRepository: IShoppingCartRepository
    {
        IRepository<ShoppingCartStore> scStoreRepo;
        IRepository<OrderNumber> orderNumberRepo;
        OrderDbContext dbContext;
        public ShoppingCartRepository(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.scStoreRepo = new EFRepository<ShoppingCartStore>(dbContext);
            this.orderNumberRepo = new EFRepository<OrderNumber>(dbContext);
        }

        public IList<OrderItem> FindOrderItem(int orderId)
        {
            return dbContext.Database.SqlQuery<OrderItem>(
                @"SELECT Id, ItemId, Qty, Price, AmountAfterDiscount, ItemCode, ItemName, UnitCode FROM OrderItems WHERE OrderId = {0}", orderId).ToList();
        }

        public model.ShoppingCart Get(string username)
        {
            ShoppingCartStore store = scStoreRepo.Where(s => s.UserId == username).FirstOrDefault();
            
            if (store == null)
            {
                return ShoppingCart.Create(username);
            }

            ShoppingCartSnapshot snapshot = JsonConvert.DeserializeObject<ShoppingCartSnapshot>(store.Payload);
            ShoppingCart sc = ShoppingCart.FromSnapshot(snapshot);
            
            return sc;
        }

        public void Save(ShoppingCart sc)
        {
            ShoppingCartSnapshot snapshot = sc.CreateSnapshot();

            ShoppingCartStore store = scStoreRepo.Where(s => s.UserId == snapshot.UserId).FirstOrDefault();
            if (store == null)
            {
                store = new ShoppingCartStore
                {
                    UserId = snapshot.UserId,
                    Payload = JsonConvert.SerializeObject(snapshot)
                };
                scStoreRepo.Add(store);
            }
            else
            {
                store.Payload = JsonConvert.SerializeObject(snapshot);
                scStoreRepo.Update(store);
            }
        }

        public OrderNumber GetOrderNumber(string branchId)
        {
            DateTime today = DateTime.Today;
            OrderNumber orderNumber = orderNumberRepo.Where(order => order.Year == today.Year && order.Month == today.Month && order.Branch == branchId).FirstOrDefault();
            if (orderNumber == null)
            {
                orderNumber = new OrderNumber(branchId, today.Year, today.Month);
            }
            return orderNumber;
        }
        public void SaveOrderNumber(OrderNumber orderNumber)
        {
            if (orderNumber.Id == 0)
            {
                orderNumberRepo.Add(orderNumber);
            }
            else
            {
                orderNumberRepo.Update(orderNumber);
            }
        }
    }
}