using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
   public class ValuePercent
    {
        public double Value { get; set; }
        public double Percentage { get; set; }
        public double alldisc { get; set; }
        public DateTime expiredate { get; set; }
    }
    public class ValuePercentitem
    {
        public double Value { get; set; }
        public double Percentage { get; set; }
        public SaleProduct product { get; set; }
    }
    public class ValueQuantity
    {
        public double Quantity { get; set; }
        public SaleProduct product { get; set; }
    }
    public class SaleHold
    {
      
        public HoldProduct product { get; set; }
    }
}
