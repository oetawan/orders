using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.model
{

    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalAmountAfterDiscount { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }

        public Order Order { get; set; }
        public int ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal AmountAfterDiscount { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitCode { get; set; }
    }
}