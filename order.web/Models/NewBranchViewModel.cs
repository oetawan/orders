﻿using order.web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace order.web.Models
{
    public class NewBranchViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Branch Code")]
        [DataType(DataType.Text)]
        public string BranchCode { get; set; }

        [Required]
        [Display(Name = "Branch Name")]
        [DataType(DataType.Text)]
        public string BranchName { get; set; }

        [Required]
        [Display(Name = "User name")]
        [DataType(DataType.EmailAddress)]
        [ValidateEmailAddress]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}