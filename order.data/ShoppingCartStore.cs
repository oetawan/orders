﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace order.data
{
    public class ShoppingCartStore
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Payload { get; set; }
    }
}