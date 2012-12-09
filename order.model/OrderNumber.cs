using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.model
{
    public class OrderNumber
    {
        public int Id { get; set; }
        public string Branch { get; private set; }
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Number { get; private set; }
        
        public OrderNumber() { }
        public OrderNumber(string branchId, int year, int month) {
            this.Branch = branchId;
            this.Year = year;
            this.Month = month;
            this.Number = 0;
        }
        public void Next() {
            Number += 1;
        }
        public string OrderNumberString()
        {
            return string.Format("{0}-{1}{2}-{3}", Branch, Year.ToString(), Month.ToString().PadLeft(2, '0'), Number.ToString());
        }
    }
}