using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
 public   class Client
    {
        public int client_id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string note { get; set; }
        public int? limitt { get; set; }
        public string address { get; set; }
        public string created_at { get; set; }
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
