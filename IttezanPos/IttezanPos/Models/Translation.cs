using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
    public class Translation
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string locale { get; set; }
    }
}
