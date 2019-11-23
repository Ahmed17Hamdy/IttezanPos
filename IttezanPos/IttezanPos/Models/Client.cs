using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
 public   class Client
    {
        [JsonProperty("client_id")]
        public int client_id { get; set; }
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("phone")]
        public string phone { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("note")]
        public string note { get; set; }
        [JsonProperty("limitt")]
        public int? limitt { get; set; }
        [JsonProperty("total_amount")]
        public double total_amount { get; set; }
        [JsonProperty("paid_amount")]
        public double paid_amount { get; set; }
        [JsonProperty("remaining")]
        public double remaining { get; set; }
        [JsonProperty("creditorit")]
        public double creditorit { get; set; }
        [JsonProperty("address")]
        public string address { get; set; }
        [JsonProperty("created_at")]
        public string created_at { get; set; }
        [JsonProperty("updated_at")]
        public string updated_at { get; set; }
    }
    public class AddClient
    {
        public bool success { get; set; }
        public Client data { get; set; }
        public string message { get; set; }
    }
    public class AddSupplier
    {
        public bool success { get; set; }
        public Supplier data { get; set; }
        public string message { get; set; }
    }
    public class AddClientError
    {
        public bool success { get; set; }
        public ErrorData data { get; set; }
    }
    public class AddSupplierError
    {
        public bool success { get; set; }
        public ErrorData data { get; set; }
    }
    public class ErrorData
    {
        public List<string> email { get; set; }
    }
    public class DelResponse
    {
        public bool success { get; set; }
        public string data { get; set; }
        public string message { get; set; }
    }

}
