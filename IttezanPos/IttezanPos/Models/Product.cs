using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
 public   class Product
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string user_id { get; set; }
        public string catname { get; set; }
        public int category_id { get; set; }
        public string  product_name { get; set; }
        public int product_id { get; set; }
        public string user { get; set; }
        public string name { get; set; }
        public string namee { get; set; }
        public string locale { get; set; }
        public string image { get; set; }
        public int purchase_price { get; set; }
        public int sale_price { get; set; }
        public int stock { get; set; }
        public string profit_percent { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Translation> translations { get; set; }
        

    }
    public class AddProduct
    {
        public bool success { get; set; }
        public string data { get; set; }
        public string message { get; set; }
    }

}
