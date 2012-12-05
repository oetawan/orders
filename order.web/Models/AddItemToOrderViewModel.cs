using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace order.web.Models
{
    public class AddItemToOrderViewModel
    {
        public int ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }

        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitCode { get; set; }
    }
}