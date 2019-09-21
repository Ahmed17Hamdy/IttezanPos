using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
 public   class Product
    {
        public int id { get; set; }
        public string catname { get; set; }
        public int category_id { get; set; }
        public string user { get; set; }
        public string image { get; set; }
        public int purchase_price { get; set; }
        public int sale_price { get; set; }
        public int stock { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<Translation> translations { get; set; }
        

    }
  
}
