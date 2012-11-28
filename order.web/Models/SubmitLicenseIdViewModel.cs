﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace order.web.Models
{
    public class SubmitLicenseIdViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string LicenseId { get; set; }
    }
}