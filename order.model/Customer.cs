using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace order.model
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "License Id")]
        [DataType(DataType.Text)]
        [StringLength(300)]
        public string LicenseId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Customer Code")]
        [DataType(DataType.Text)]
        public string CustomerCode { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        [DataType(DataType.Text)]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name= "Service Bus Namespace")]
        [DataType(DataType.Text)]
        [StringLength(300)]
        public string ServiceBusNamespace { get; set; }

        [Required]
        [Display(Name = "Issuer")]
        [DataType(DataType.Text)]
        public string Issuer { get; set; }

        [Required]
        [Display(Name = "Secret Key")]
        [DataType(DataType.Text)]
        public string SecretKey { get; set; }
        
        public bool Registered { get; set; }
    }
}