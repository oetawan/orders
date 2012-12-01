using order.snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.model
{
    public class ShoppingCart
    {
        private string userId = string.Empty;
        private List<ShoppingCartItem> items = new List<ShoppingCartItem>();
        private decimal totalAmountAfterDiscount = 0;

        private ShoppingCart() { }

        private ShoppingCart(string userId)
        {
            this.userId = userId;
        }

        private ShoppingCart(ShoppingCartSnapshot snapshot)
        {
            this.userId = snapshot.UserId;
            this.items = new List<ShoppingCartItem>();
            this.totalAmountAfterDiscount = snapshot.TotalAmountfterDiscount;
            snapshot.Items.ForEach(itemSnapshot => {
                items.Add(ShoppingCartItem.FromSnapshot(itemSnapshot));
            });
        }

        public ShoppingCartSnapshot CreateSnapshot()
        {
            List<ShoppingCartItemSnapshot> itemsSnapshot = new List<ShoppingCartItemSnapshot>();
            
            items.ForEach(item => {
                itemsSnapshot.Add(item.CreateSnapshot());
            });

            ShoppingCartSnapshot snapshot = new ShoppingCartSnapshot { 
                TotalAmountfterDiscount = this.totalAmountAfterDiscount,
                Items = itemsSnapshot,
                UserId = this.userId
            };

            return snapshot;
        }

        public static ShoppingCart FromSnapshot(ShoppingCartSnapshot snapshot)
        {
            return new ShoppingCart(snapshot);
        }

        #region Behaviour

        public static ShoppingCart Create(string userId)
        {
            return new ShoppingCart(userId);
        }

        public void AddItem(AddItemCommand param)
        {
            ShoppingCartItem existingItem = items.Where(item => item.CreateSnapshot().ItemId == param.ItemId).FirstOrDefault();
            if (existingItem != null)
                items.Remove(existingItem);
            
            items.Add(ShoppingCartItem.Create(param.ItemId, param.Qty, param.Price));
            this.totalAmountAfterDiscount = items.Sum(item => item.CreateSnapshot().AmountAfterDiscount);
        }

        #endregion

        #region Command

        public class AddItemCommand
        {
            public int ItemId { get; set; }
            public decimal Qty { get; set; }
            public decimal Price { get; set; }
        }

        #endregion

        #region Entity

        public class ShoppingCartItem
        {
            private int itemId;
            private decimal qty;
            private decimal price;
            private decimal amountAfterDiscount;

            #region Behaviour

            private ShoppingCartItem(int itemId, decimal qty, decimal price)
            {
                this.itemId = itemId;
                this.qty = qty;
                this.price = price;
                this.amountAfterDiscount = qty * price;
            }

            public static ShoppingCartItem Create(int itemId, decimal qty, decimal price)
            {
                return new ShoppingCartItem(itemId, qty, price);
            }

            #endregion

            private ShoppingCartItem() { }

            private ShoppingCartItem(ShoppingCartItemSnapshot snapshot)
            {
                this.itemId = snapshot.ItemId;
                this.qty = snapshot.Qty;
                this.price = snapshot.Price;
                this.amountAfterDiscount = snapshot.AmountAfterDiscount;
            }

            public ShoppingCartItemSnapshot CreateSnapshot()
            {
                return new ShoppingCartItemSnapshot
                {
                    ItemId = this.itemId,
                    Price = this.price,
                    Qty = this.qty,
                    AmountAfterDiscount = this.amountAfterDiscount
                };
            }

            public static ShoppingCartItem FromSnapshot(ShoppingCartItemSnapshot snapshot)
            {
                return new ShoppingCartItem(snapshot);
            }
        }

        #endregion
    }
}