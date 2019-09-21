using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
    public class Category
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("created_at")]
        public string created_at { get; set; }
        [JsonProperty("updated_at")]
        public string updated_at { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("translations")]
        public List<Translation> translations { get; set; }
    }
    public class EachCategory
    {
        [JsonProperty("category")]
        public Category category { get; set; }
        [JsonProperty("products")]
        public Product products { get; set; }
    }
    public class Message
    {
        [JsonProperty("clients")]
        public List<Client> clients { get; set; }
        [JsonProperty("suppliers")]
        public List<Supplier> suppliers { get; set; }
        [JsonProperty("users")]
        public List<User> users { get; set; }
        [JsonProperty("each_category")]
        public List<EachCategory> each_category { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("success")]
        public bool success { get; set; }
        [JsonProperty("data")]
        public string data { get; set; }
        [JsonProperty("message")]
        public Message message { get; set; }
    }
}
