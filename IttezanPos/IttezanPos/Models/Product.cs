using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
 public   class Product
    {
        [PrimaryKey]
        public int id { get; set; }
        [ForeignKey(typeof(Category2))]
        public int category2Id { get; set; }      
        public string user_id { get; set; }
        public string catname { get; set; }
        public int category_id { get; set; }
        public int purchasing_order { get; set; }
        public string  product_name { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public string discount { get; set; }
        public string user { get; set; }
        public string name { get; set; }
        public string namee { get; set; }
        public string locale { get; set; }
        public string image { get; set; }
        public double purchase_price { get; set; }
        public double total_price { get; set; }
        public double sale_price { get; set; }
        public int stock { get; set; }
        public string profit_percent { get; set; }
        public string description { get; set; }
        public DateTimeOffset? created_at { get; set; }
        public string expiration_date { get; set; }
        public DateTimeOffset? updated_at { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All),Ignore]
        public List<Translation> translations { get; set; }
        

    }
    public class AddProduct
    {
        public bool success { get; set; }
        public string data { get; set; }
        public string message { get; set; }
    }
   public class SaleProduct : Product
    {
        public int order_id { get; set; }
       
        public int client_id { get; set; }
        public string payment_type { get; set; }
        public string total_price { get; set; }
        public string amount_paid { get; set; }
        public int remaining_amount { get; set; }
    }
}
