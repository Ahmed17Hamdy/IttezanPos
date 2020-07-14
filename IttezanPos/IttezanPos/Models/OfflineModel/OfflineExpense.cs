using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models.OfflineModel
{
    public  class OfflineExpense
    {
        public bool success { get; set; }
        public List<Datum> data { get; set; }
        public string message { get; set; }
    }
    public class Datum
    {
        public int user_id { get; set; }
        public int amount { get; set; }
        public string statement { get; set; }
        public string date { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int id { get; set; }
    }

}
