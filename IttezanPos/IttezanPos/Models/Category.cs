using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
    public class Category
    {

        [ForeignKey(typeof(Category2))]
        public int categoryId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Category2 category { get; set; }
    }
    public class Settings
    {
        public int id { get; set; }
        public string name { get; set; }
        public string enname { get; set; }
        public string about { get; set; }
        public string enabout { get; set; }
        public string policy { get; set; }
        public string enpolicy { get; set; }
        public string logo { get; set; }
        public object how_used { get; set; }
        public object connect_us { get; set; }
        public string phone { get; set; }
        public object mobile { get; set; }
        public string Address { get; set; }
        public string email { get; set; }
        public string price1 { get; set; }
        public string price2 { get; set; }
        public string price3 { get; set; }
        public object client_video { get; set; }
        public object company_video { get; set; }
        public object comp_cond { get; set; }
        public object ecomp_cond { get; set; }
        public object comp_policy { get; set; }
        public object ecomp_policy { get; set; }
        public object comp_connect { get; set; }
        public object ecomp_connect { get; set; }
        public object created_at { get; set; }
        public string updated_at { get; set; }
    }
    public class Category2
    {
        [PrimaryKey]
        [JsonProperty("id")]
        public int id { get; set; }
        [OneToMany]
        public List<Category> categories { get; set; }
       
        [JsonProperty("created_at")]
        public string created_at { get; set; }
        [JsonProperty("updated_at")]
        public string updated_at { get; set; }
        [JsonProperty("list_of_products")]
        public List<Product> list_of_products { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        [JsonProperty("translations")]
        public List<Translation> translations { get; set; }
    }


    public class Message
    {
        [JsonProperty("settings")]
        public Settings settings { get; set; }
        [JsonProperty("clients")]
        public List<Client> clients { get; set; }

        [JsonProperty("suppliers")]
        public List<Supplier> suppliers { get; set; }
        [JsonProperty("box")]

        public Box box { get; set; }
        [JsonProperty("payments")]
        public List<Payment> payments { get; set; }
        [JsonProperty("users")]
        public List<User> users { get; set; }

        [JsonProperty("categories")]
        public List<Category> categories { get; set; }
    }
    public class Payment
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("en_name")]
        public string en_name { get; set; }
        [JsonProperty("payment_type_number")]
        public int payment_type_number { get; set; }
        [JsonProperty("created_at")]
        public string created_at { get; set; }
        [JsonProperty("updated_at")]
        public string updated_at { get; set; }
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
