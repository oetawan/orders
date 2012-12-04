using order.data.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.service
{
    public class CommandExecutor
    {
        IOrderUow orderUow;
        public CommandExecutor(IOrderUow orderUow)
        {
            this.orderUow = orderUow;
        }

        public void Execute(Action<IOrderUow> action)
        {
            if (action != null)
            {
                action(this.orderUow);
                this.orderUow.Commit();
            }
        }
    }
}