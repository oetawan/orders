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

        public void AddItem(AddItemCommand cmd)
        {
            ShoppingCartItem existingItem = items.Where(item => item.CreateSnapshot().ItemId == cmd.ItemId).FirstOrDefault();
            if (existingItem != null)
                items.Remove(existingItem);
            
            items.Add(ShoppingCartItem.Create(cmd.ItemId, cmd.Qty, cmd.Price, cmd.ItemCode, cmd.ItemName, cmd.UnitCode));
            this.totalAmountAfterDiscount = items.Sum(item => item.CreateSnapshot().AmountAfterDiscount);
        }

        public void RemoveItem(RemoveItemCommand cmd)
        {
            ShoppingCartItem existingItem = items.Where(item => item.CreateSnapshot().ItemId == cmd.ItemId).FirstOrDefault();
            if (existingItem != null)
                items.Remove(existingItem);

            this.totalAmountAfterDiscount = items.Sum(item => item.CreateSnapshot().AmountAfterDiscount);
        }

        #endregion

        #region Command

        public class AddItemCommand
        {
            public int ItemId { get; set; }
            public decimal Qty { get; set; }
            public decimal Price { get; set; }
            public string Username { get; set; }

            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string UnitCode { get; set; }
        }

        public class RemoveItemCommand
        {
            public int ItemId { get; set; }
            public string Username { get; set; }
        }

        #endregion

        #region Entity

        public class ShoppingCartItem
        {
            private int itemId;
            private decimal qty;
            private decimal price;
            private decimal amountAfterDiscount;

            private string itemCode;
            private string itemName;
            private string unitCode;

            #region Behaviour

            private ShoppingCartItem(int itemId, decimal qty, decimal price, string itemCode, string itemName, string unitCode)
            {
                this.itemId = itemId;
                this.qty = qty;
                this.price = price;
                this.amountAfterDiscount = qty * price;

                this.itemCode = itemCode;
                this.itemName = itemName;
                this.unitCode = unitCode;
            }

            public static ShoppingCartItem Create(int itemId, decimal qty, decimal price, string itemCode, string itemName, string unitCode)
            {
                return new ShoppingCartItem(itemId, qty, price, itemCode, itemName, unitCode);
            }

            #endregion

            private ShoppingCartItem() { }

            private ShoppingCartItem(ShoppingCartItemSnapshot snapshot)
            {
                this.itemId = snapshot.ItemId;
                this.qty = snapshot.Qty;
                this.price = snapshot.Price;
                this.amountAfterDiscount = snapshot.AmountAfterDiscount;

                itemCode = snapshot.ItemCode;
                itemName = snapshot.ItemName;
                unitCode = snapshot.UnitCode;
            }

            public ShoppingCartItemSnapshot CreateSnapshot()
            {
                return new ShoppingCartItemSnapshot
                {
                    ItemId = this.itemId,
                    Price = this.price,
                    Qty = this.qty,
                    AmountAfterDiscount = this.amountAfterDiscount,
                    
                    ItemCode = this.itemCode,
                    ItemName = this.itemName,
                    UnitCode = this.unitCode
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