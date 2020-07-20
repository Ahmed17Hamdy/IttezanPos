using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models
{
  public  class Box
    {
        [PrimaryKey,AutoIncrement]
        public int id { get; set; }
        public int type_operation { get; set; }
        public string balance { get; set; }
        public string amount { get; set; }
        public string note { get; set; }
        public string date { get; set; }

        public int add_sales_clients { get; set; }
        public int disc_purchasing_suppliers { get; set; }
        public int disc_expenses { get; set; }
    }
}
