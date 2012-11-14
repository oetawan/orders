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
        public string LicenseId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Branch Code")]
        [DataType(DataType.Text)]
        public string BranchCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Customer Code")]
        [DataType(DataType.Text)]
        public string CustomerCode { get; set; }

        [Required]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public bool Registered { get; set; }
    }
}