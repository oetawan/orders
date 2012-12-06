using order.model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace order.data.Configuration
{
    public class OrderConfiguration: EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            HasMany(o => o.Items)
            .WithRequired(item => item.Order)
            .Map(x => x.MapKey("OrderId"));
        }
    }
}