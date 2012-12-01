using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using order.snapshot;

namespace order.model.specs.ShoppingCartSpecs
{
    [TestClass]
    public class ShoppingCartSpecs
    {
        ShoppingCart sc;

        [TestInitialize]
        public void Context()
        {
            sc = ShoppingCart.Create("oetawan@gmail.com");
        }

        [TestMethod]
        public void Add_item_to_shopping_cart()
        {
            sc.AddItem(new ShoppingCart.AddItemCommand { ItemId = 1, Qty = 10, Price = 1000000 });

            ShoppingCartSnapshot snapshot = sc.CreateSnapshot();

            Assert.AreEqual(10000000m, snapshot.TotalAmountfterDiscount);
            Assert.AreEqual(10000000m, snapshot.Items[0].AmountAfterDiscount);
            Assert.AreEqual(10, snapshot.Items[0].Qty);
            Assert.AreEqual(1000000m, snapshot.Items[0].Price);
        }

        [TestMethod]
        public void Add_multiple_items_to_shopping_cart()
        {
            sc.AddItem(new ShoppingCart.AddItemCommand { ItemId = 1, Qty = 10, Price = 1000000 });
            sc.AddItem(new ShoppingCart.AddItemCommand { ItemId = 2, Qty = 20, Price = 500000 });

            ShoppingCartSnapshot snapshot = sc.CreateSnapshot();

            Assert.AreEqual(20000000m, snapshot.TotalAmountfterDiscount);
        }
    }
}