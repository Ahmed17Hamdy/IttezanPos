using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
    public class Supplier
    {
        public int supplier_id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        [JsonProperty("enname")]
        public string enname { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string note { get; set; }
        [JsonProperty("total_amount")]
        public double total_amount { get; set; }
        [JsonProperty("paid_amount")]
        public double paid_amount { get; set; }
        [JsonProperty("remaining")]
        public double remaining { get; set; }
        [JsonProperty("creditorit")]
        public double creditorit { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
    public class SuplierTotalAmount
    {
        public string name { get; set; }
        public double remaining { get; set; }
        public double creditorit { get; set; }
        public double paid_amount { get; set; }
        public double total_amount { get; set; }
      
    }
}
