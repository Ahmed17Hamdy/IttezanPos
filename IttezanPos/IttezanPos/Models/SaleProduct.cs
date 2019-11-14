using IttezanPos.Views.SalesPages;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IttezanPos.Models
{
   public class OrderItem
    {
       
        public string total_price { get; set; }
       
        public string amount_paid { get; set; }
     
        public string discount { get; set; }
        
        public string total_price_after_discount { get; set; }
    
        public string client_id { get; set; }
       
        public int user_id { get; set; }
     
        public string payment_type { get; set; }
     
     
        public List<Product> products { get; set; }
    }
    public class Purchaseitem
    {

        public int purchasing_order { get; set; }
        public string payment_type { get; set; }

        public int user_id { get; set; }
        public string supplier_id { get; set; }
        public string discount { get; set; }
        public string total_price { get; set; }
        public int total_price_after_discount { get; set; }


        public List<Product> products { get; set; }
    }
    public class sub
    {
        public int id { get; set; }
        public int quantity { get; set; }
    }
    public class SaleObject
    {
        public bool success { get; set; }
        public string data { get; set; }
        public List<Purchaseitem> message { get; set; }
    }
}
