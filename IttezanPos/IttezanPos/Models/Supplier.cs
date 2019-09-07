using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
    public class Supplier
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int number { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
