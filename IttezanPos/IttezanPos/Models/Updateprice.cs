using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
  public  class Updateprice
    {
        public int up_down { get; set; }
        public int purchase_sale { get; set; }
        public int value_ratio { get; set; }
        public int category_id { get; set; }
        public double amount { get; set; }
      
    }
    public class RootObjectUpdate
    {
        [JsonProperty("success")]
        public bool success { get; set; }
        [JsonProperty("data")]
        public string data { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
    }
}
