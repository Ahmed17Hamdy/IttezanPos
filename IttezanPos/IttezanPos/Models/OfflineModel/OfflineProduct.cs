using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models.OfflineModel
{
  public  class OfflineProduct
    {
      
        [JsonProperty("category_id")]
        [PrimaryKey]
        public int category_id { get; set; }
        public string name { get; set; }
        public string enname { get; set; }
        public string barcode { get; set; }
        public double purchase_price { get; set; }
        public double sale_price { get; set; }
        public double stock { get; set; }
        public string image { get; set; }
        public int user_id { get; set; }
        public string notes { get; set; }
        public string expiredate { get; set; }
        public int trackinstore { get; set; }
        public int type { get; set; }
        public string description { get; set; }

    }
    public class UpdateOfflineProduct
    {
        [JsonProperty("product_id")]
        [PrimaryKey, AutoIncrement]
        public int product_id { get; set; }
        [JsonProperty("category_id")]

        public int category_id { get; set; }
        public string name { get; set; }
        public string enname { get; set; }
        public string barcode { get; set; }
        public double purchase_price { get; set; }
        public double sale_price { get; set; }
        public double stock { get; set; }
        public string image { get; set; }
        public int user_id { get; set; }
        public string notes { get; set; }
        public string expiredate { get; set; }
        public int trackinstore { get; set; }
        public int type { get; set; }
        public string description { get; set; }

    }
    public class DeleteOfflineProduct : Product
    {

    }
}
