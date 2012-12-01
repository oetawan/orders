using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.snapshot
{
    public class ShoppingCartSnapshot
    {
        public string UserId { get; set; }
        public decimal TotalAmountfterDiscount { get; set; }
        public List<ShoppingCartItemSnapshot> Items { get; set; }
    }

    public class ShoppingCartItemSnapshot
    {
        public int ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal AmountAfterDiscount { get; set; }
    }
}