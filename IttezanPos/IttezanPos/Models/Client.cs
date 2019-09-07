using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
 public   class Client
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("phone")]
        public object phone { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("note")]
        public string note { get; set; }
        [JsonProperty("limitt")]
        public int limitt { get; set; }
        [JsonProperty("address")]
        public string address { get; set; }
        [JsonProperty("created_at")]
        public string created_at { get; set; }
        [JsonProperty("updated_at")]
        public string updated_at { get; set; }
    }
}
