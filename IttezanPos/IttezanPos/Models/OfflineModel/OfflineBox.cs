using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models.OfflineModel
{
    public class OfflineBox
    {
        public bool success { get; set; }
        public string data { get; set; }
        public List<Offlineforbox> message { get; set; }
    }
    public class Offlineforbox
    {
        public int type_operation { get; set; }
        public int amount { get; set; }
        public string date { get; set; }
        public string note { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int id { get; set; }
    }
}
