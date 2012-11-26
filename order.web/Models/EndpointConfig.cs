using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace order.web.Models
{
    public class EndpointConfig
    {
        public string ServiceBusNamespace { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}