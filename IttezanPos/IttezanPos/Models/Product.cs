using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
 public   class Product
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public int user_id   { get; set; }
        public string image { get; set; }
        public int purchase_price { get; set; }
        public int sale_price { get; set; }
        public int stock { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string image_path { get; set; }
        public string profit_percent { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Translation> translations { get; set; }
     
    }
  
}
