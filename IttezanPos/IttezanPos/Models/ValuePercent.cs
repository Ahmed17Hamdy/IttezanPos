using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
   public class ValuePercent
    {
        public string Value { get; set; }
        public string Percentage { get; set; }
        public DateTime expiredate { get; set; }
    }
    public class ValueQuantity
    {
        public Double Quantity { get; set; }
        public Product product { get; set; }
    }
}
