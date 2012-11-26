using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.model
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Branch Code")]
        [DataType(DataType.Text)]
        public string BranchCode { get; set; }

        [Required]
        [Display(Name = "Branch Name")]
        [DataType(DataType.Text)]
        public string BranchName { get; set; }
    }
}