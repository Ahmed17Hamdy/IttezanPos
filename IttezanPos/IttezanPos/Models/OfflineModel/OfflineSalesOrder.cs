using System;
using System.Collections.Generic;
using System.Text;

namespace IttezanPos.Models.OfflineModel
{
    public class OfflineSalesOrder
    {
        public double total_price { get; set; }
        public double amount_paid { get; set; }
        public string payment_type { get; set; }
        public double discount { get; set; }
        public string user_id { get; set; }
        public string client_id { get; set; }
        public List<SaleProductoff> products { get; set; }
    }
    public class SaleProductoff
    {
        public int id { get; set; }
        public double quantity { get; set; }
        public double total_price { get; set; }
        public double sale_price { get; set; }
        public double purchase_price { get; set; }
        public DateTimeOffset? expiration_date { get; set; }
    }
}
