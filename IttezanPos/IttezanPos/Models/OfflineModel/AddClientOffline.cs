using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models.OfflineModel
{
  public  class AddClientOffline : Client
    {
    }
    public class UpdateClientOffline 
    {
        [JsonProperty("client_id")]
        public int client_id { get; set; }
        [JsonProperty("id")]
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("enname")]
        public string enname { get; set; }
        [JsonProperty("phone")]
        public string phone { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("note")]
        public string note { get; set; }
        [JsonProperty("limitt")]
        public double limitt { get; set; }
       
        [JsonProperty("address")]
        public string address { get; set; }
       
    }
    public class DeleteClientOffline : Client
    {
    }
    public class OfflineClientAdded
    {
        public bool success { get; set; }
        public List<Client> data { get; set; }
        public string message { get; set; }
    }
}
