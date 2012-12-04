using order.data.contract;
using order.model;
using order.snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace order.data
{
    public class ShoppingCartRepository: IShoppingCartRepository
    {
        IRepository<ShoppingCartStore> scStoreRepo;
        OrderDbContext dbContext;
        public ShoppingCartRepository(OrderDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.scStoreRepo = new EFRepository<ShoppingCartStore>(dbContext);
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
    }
}