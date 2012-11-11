using order.web.Models;
using order.web.net.webservicex;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace order.web.Filters
{
    public class ValidateEmailAddressAttribute : ValidationAttribute
    {
        public bool isEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult result = new ValidationResult("Unknown error");
            try
            {
                string email = value == null ? string.Empty : value.ToString();
                if (!isEmail(email))
                    result = new ValidationResult(value.ToString() + " is not valid email address");
                else
                    result = ValidationResult.Success;

                return result;
            }
            catch (Exception ex)
            {
                result = new ValidationResult(ex.Message);
                return result;
            }
        }
    }
}