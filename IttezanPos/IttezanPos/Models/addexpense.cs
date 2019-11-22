using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
 public   class addexpense
    {
        public double amount { get; set; }
        public string date { get; set; }
        public string statement { get; set; }
        public int user_id { get; set; }
      
    }
    public class AddexpenseData
    {
        public string user_id { get; set; }
        public string amount { get; set; }
        public string statement { get; set; }
        public string date { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int id { get; set; }
    }
    public class ExpenseRootObject
    {
        public bool success { get; set; }
        public AddexpenseData data { get; set; }
        public string message { get; set; }
    }
}
