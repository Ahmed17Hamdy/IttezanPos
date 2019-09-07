using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public object email_verified_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string image_path { get; set; }
    }
}
