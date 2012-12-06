using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace order.data
{
    public class ShoppingCartStore
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        public string UserId { get; set; }
        public string Payload { get; set; }
    }
}