using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace order.model
{
    public class UserCustomerMapping
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(300)]
        public string Username { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}