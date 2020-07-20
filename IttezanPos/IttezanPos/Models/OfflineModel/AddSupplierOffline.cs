using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models.OfflineModel
{
    public class AddSupplierOffline : Supplier
    {
    }
    public class UpdateSupplierOffline 
    {
        public int supplier_id { get; set; }
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
       
        public string enname { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string note { get; set; }
        
    }
    public class DeleteSupplierOffline : Supplier
    {
    }
}
